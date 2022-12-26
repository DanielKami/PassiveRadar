


__kernel void Kernel(		__global float* output_re,
									__global float* output_im, 
									__global float* coniugate,
									__global float* SIN,
									__global float* COS,
									__local float* partial_sums_re,
									__local float* partial_sums_im,
									const     unsigned int OutputLenght, 
									const     unsigned int BufferSize, 
									const     unsigned int Buff_Col,
									const     unsigned int HalfColumn,
									const     unsigned int OutLenghtCol
									) 
{
    int lid = get_local_id(0);
    int group_size = get_local_size(0);
	int global_id0 = get_global_id(0); //f
	int global_id1 = get_global_id(1); //mi in columns
	int global_id2 = get_global_id(2); //tau in rows

	    int Hf   =  global_id0 + HalfColumn;
		int tsf  =  global_id2 * Buff_Col + Hf;

        partial_sums_im[lid] = partial_sums_re[lid] = coniugate[global_id1 * BufferSize +  Hf];  
		partial_sums_re[lid] *= COS[tsf];
        partial_sums_im[lid] *= SIN[tsf];        

                       
	///////////////////////////////////////////////////////////////////////////////////

    barrier(CLK_LOCAL_MEM_FENCE);
    for(int i = group_size/2; i>0; i >>= 1) {
        if(lid < i) {
            partial_sums_re[lid] += partial_sums_re[lid +i];
			partial_sums_im[lid] += partial_sums_im[lid +i];
        }
        barrier(CLK_LOCAL_MEM_FENCE);
    }

    if(lid == 0) {
	    int position = get_group_id(0) + 
		               global_id1 * get_num_groups(0) +
					   global_id2 * get_num_groups(0)*get_global_size(1);

		output_re[position] = partial_sums_re[0];
		output_im[position] = partial_sums_im[0];
    }
}

