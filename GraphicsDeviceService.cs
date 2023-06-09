#region File Description
//-----------------------------------------------------------------------------
// GraphicsDeviceService.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using System.Windows.Forms;
#endregion

// The IGraphicsDeviceService interface requires a DeviceCreated event, but we
// always just create the device inside our constructor, so we have no place to
// raise that event. The C# compiler warns us that the event is never used, but
// we don't care so we just disable this warning.
#pragma warning disable 67

namespace PasiveRadar
{
    /// <summary>
    /// Helper class responsible for creating and managing the GraphicsDevice.
    /// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
    /// so even though there can be many controls, there will only ever be a single
    /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
    /// interface, which provides notification events for when the device is reset
    /// or disposed.
    /// </summary>
    public class GraphicsDeviceService : IGraphicsDeviceService, IDisposable
    {
        #region Fields


        // Singleton device service instance.
        static GraphicsDeviceService singletonInstance;


        // Keep track of how many controls are sharing the singletonInstance.
        static int referenceCount;
        PresentationParameters presentation_parameters;

        #endregion


        /// <summary>
        /// Constructor is private, because this is a singleton class:
        /// client controls should use the public AddRef method instead.
        /// </summary>
        public GraphicsDeviceService(IntPtr windowHandle, int width, int height)
        {
            CreateDevice(windowHandle, width, height);
        }





        public void CreateDevice(IntPtr windowHandle, int width, int height)
        {
            // Create Presentation Parameters
            presentation_parameters = new PresentationParameters();
            // pp.BackBufferCount = 1;
            presentation_parameters.IsFullScreen = false;
            // pp.SwapEffect = SwapEffect.Discard;
            presentation_parameters.BackBufferWidth = Math.Max(width, 1);
            presentation_parameters.BackBufferHeight = Math.Max(height, 1);
            // pp.AutoDepthStencilFormat = DepthFormat.Depth24Stencil8;
            // pp.EnableAutoDepthStencil = true;
            presentation_parameters.PresentationInterval = PresentInterval.Default;
            presentation_parameters.BackBufferFormat = SurfaceFormat.Color;
            // pp.MultiSampleType = MultiSampleType.None;
            presentation_parameters.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            presentation_parameters.DeviceWindowHandle = windowHandle;
            presentation_parameters.PresentationInterval = PresentInterval.Immediate;

            // Create device
            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach,
                //   this.panelViewport.Handle,
                //  CreateOptions.HardwareVertexProcessing,
                presentation_parameters);


        }
        /// <summary>
        /// Gets a reference to the singleton instance.
        /// </summary>
        public static GraphicsDeviceService AddRef(IntPtr windowHandle,
                                                   int width, int height)
        {
            // Increment the "how many controls sharing the device" reference count.
            if (Interlocked.Increment(ref referenceCount) == 1)
            {
                // If this is the first control to start using the
                // device, we must create the singleton instance.
                singletonInstance = new GraphicsDeviceService(windowHandle,
                                                              width, height);
            }

            return singletonInstance;
        }


        /// <summary>
        /// Releases a reference to the singleton instance.
        /// </summary>
        public void Release(bool disposing)
        {
            // Decrement the "how many controls sharing the device" reference count.
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
                // If this is the last control to finish using the
                // device, we should dispose the singleton instance.
                if (disposing)
                {
                    if (DeviceDisposing != null)
                        DeviceDisposing(this, EventArgs.Empty);

                    graphicsDevice.Dispose();
                }

                graphicsDevice = null;
            }
        }


        /// <summary>
        /// Resets the graphics device to whichever is bigger out of the specified
        /// resolution or its current size. This behavior means the device will
        /// demand-grow to the largest of all its GraphicsDeviceControl clients.
        /// </summary>
        public void ResetDevice(int width, int height)
        {

            ResetingDevice();


            if (presentation_parameters == null) 
                MessageBox.Show("Presentation parameters is null.", "Important Message");
            try
            {
                graphicsDevice.Reset(presentation_parameters);
            }
            catch
            {
                Release(true);
                ResetingDevice();

                DeviceReset(this, EventArgs.Empty);
                //graphicsDevice.Reset(presentation_parameters);

            }

            presentation_parameters.BackBufferWidth = Math.Max(presentation_parameters.BackBufferWidth, width);
            presentation_parameters.BackBufferHeight = Math.Max(presentation_parameters.BackBufferHeight, height);



            if (DeviceReset != null)
                try
                {
                    DeviceReset(this, EventArgs.Empty);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Reset device:" + e, "Important Message");
                }
        }

        public void ResetingDevice()
        {

            if (DeviceResetting != null)
                DeviceResetting(this, EventArgs.Empty);
        }
        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        GraphicsDevice graphicsDevice;


        // IGraphicsDeviceService events.
        public event EventHandler<System.EventArgs> DeviceCreated;
        public event EventHandler<System.EventArgs> DeviceDisposing;
        public event EventHandler<System.EventArgs> DeviceReset;
        public event EventHandler<System.EventArgs> DeviceResetting;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                graphicsDevice.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
