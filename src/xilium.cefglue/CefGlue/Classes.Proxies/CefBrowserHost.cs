namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Class used to represent the browser process aspects of a browser window. The
    /// methods of this class can only be called in the browser process. They may be
    /// called on any thread in that process unless otherwise indicated in the
    /// comments.
    /// </summary>
    public sealed unsafe partial class CefBrowserHost
    {
        /// <summary>
        /// Create a new browser window using the window parameters specified by
        /// |windowInfo|. All values will be copied internally and the actual window
        /// will be created on the UI thread. This method can be called on any browser
        /// process thread and will not block.
        /// </summary>
        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url)
        {
            if (windowInfo == null) throw new ArgumentNullException("windowInfo");
            if (client == null) throw new ArgumentNullException("client");
            if (settings == null) throw new ArgumentNullException("settings");

            var n_windowInfo = windowInfo.ToNative();
            var n_client = client.ToNative();
            var n_settings = settings.ToNative();

            fixed (char* url_ptr = url)
            {
                cef_string_t n_url = new cef_string_t(url_ptr, url != null ? url.Length : 0);
                var n_success = cef_browser_host_t.create_browser(n_windowInfo, n_client, &n_url, n_settings);
                if (n_success != 1) throw ExceptionBuilder.FailedToCreateBrowser();
            }

            // TODO: free n_ structs
        }

        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, Uri url)
        {
            CreateBrowser(windowInfo, client, settings, url.ToString());
        }

        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings)
        {
            CreateBrowser(windowInfo, client, settings, string.Empty);
        }

        /// <summary>
        /// Create a new browser window using the window parameters specified by
        /// |windowInfo|. This method can only be called on the browser process UI
        /// thread.
        /// </summary>
        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url)
        {
            if (windowInfo == null) throw new ArgumentNullException("windowInfo");
            if (client == null) throw new ArgumentNullException("client");
            if (settings == null) throw new ArgumentNullException("settings");

            var n_windowInfo = windowInfo.ToNative();
            var n_client = client.ToNative();
            var n_settings = settings.ToNative();

            fixed (char* url_ptr = url)
            {
                cef_string_t n_url = new cef_string_t(url_ptr, url != null ? url.Length : 0);
                var n_browser = cef_browser_host_t.create_browser_sync(n_windowInfo, n_client, &n_url, n_settings);
                return CefBrowser.FromNative(n_browser);
            }
        }

        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, Uri url)
        {
            return CreateBrowserSync(windowInfo, client, settings, url.ToString());
        }

        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings)
        {
            return CreateBrowserSync(windowInfo, client, settings, string.Empty);
        }

        /// <summary>
        /// Returns the hosted browser object.
        /// </summary>
        public CefBrowser GetBrowser()
        {
            return CefBrowser.FromNative(cef_browser_host_t.get_browser(_self));
        }

        /// <summary>
        /// Call this method before destroying a contained browser window. This method
        /// performs any internal cleanup that may be needed before the browser window
        /// is destroyed.
        /// </summary>
        public void ParentWindowWillClose()
        {
            cef_browser_host_t.parent_window_will_close(_self);
        }

        /// <summary>
        /// Closes this browser window.
        /// </summary>
        public void CloseBrowser()
        {
            cef_browser_host_t.close_browser(_self);
        }

        /// <summary>
        /// Set focus for the browser window. If |enable| is true focus will be set to
        /// the window. Otherwise, focus will be removed.
        /// </summary>
        public void SetFocus(bool enable)
        {
            cef_browser_host_t.set_focus(_self, enable ? 1 : 0);
        }

        /// <summary>
        /// Retrieve the window handle for this browser.
        /// </summary>
        public IntPtr GetWindowHandle()
        {
            return cef_browser_host_t.get_window_handle(_self);
        }

        /// <summary>
        /// Retrieve the window handle of the browser that opened this browser. Will
        /// return NULL for non-popup windows. This method can be used in combination
        /// with custom handling of modal windows.
        /// </summary>
        public IntPtr GetOpenerWindowHandle()
        {
            return cef_browser_host_t.get_opener_window_handle(_self);
        }

        /// <summary>
        /// Returns the client for this browser.
        /// </summary>
        public CefClient GetClient()
        {
            return CefClient.FromNative(
                cef_browser_host_t.get_client(_self)
                );
        }

        /// <summary>
        /// Returns the DevTools URL for this browser. If |http_scheme| is true the
        /// returned URL will use the http scheme instead of the chrome-devtools
        /// scheme. Remote debugging can be enabled by specifying the
        /// "remote-debugging-port" command-line flag or by setting the
        /// CefSettings.remote_debugging_port value. If remote debugging is not enabled
        /// this method will return an empty string.
        /// </summary>
        public string GetDevToolsUrl(bool httpScheme)
        {
            var n_url = cef_browser_host_t.get_dev_tools_url(_self, httpScheme ? 1 : 0);
            return cef_string_userfree.ToString(n_url);
        }

        /// <summary>
        /// Get the current zoom level. The default zoom level is 0.0. This method can
        /// only be called on the UI thread.
        /// </summary>
        public double GetZoomLevel()
        {
            return cef_browser_host_t.get_zoom_level(_self);
        }

        /// <summary>
        /// Change the zoom level to the specified value. Specify 0.0 to reset the
        /// zoom level. If called on the UI thread the change will be applied
        /// immediately. Otherwise, the change will be applied asynchronously on the
        /// UI thread.
        /// </summary>
        public void SetZoomLevel(double value)
        {
            cef_browser_host_t.set_zoom_level(_self, value);
        }

        /// <summary>
        /// Call to run a file chooser dialog. Only a single file chooser dialog may be
        /// pending at any given time. |mode| represents the type of dialog to display.
        /// |title| to the title to be used for the dialog and may be empty to show the
        /// default title ("Open" or "Save" depending on the mode). |default_file_name|
        /// is the default file name to select in the dialog. |accept_types| is a list
        /// of valid lower-cased MIME types or file extensions specified in an input
        /// element and is used to restrict selectable files to such types. |callback|
        /// will be executed after the dialog is dismissed or immediately if another
        /// dialog is already pending. The dialog will be initiated asynchronously on
        /// the UI thread.
        /// </summary>
        public void RunFileDialog(CefFileDialogMode mode, string title, string defaultFileName, string[] acceptTypes, CefRunFileDialogCallback callback)
        {
            fixed (char* title_ptr = title)
            fixed (char* defaultFileName_ptr = defaultFileName)
            {
                var n_title = new cef_string_t(title_ptr, title != null ? title.Length : 0);
                var n_defaultFileName = new cef_string_t(defaultFileName_ptr, defaultFileName != null ? defaultFileName.Length : 0);
                var n_acceptTypes = cef_string_list.From(acceptTypes);

                cef_browser_host_t.run_file_dialog(_self, mode, &n_title, &n_defaultFileName, n_acceptTypes, callback.ToNative());

                libcef.string_list_free(n_acceptTypes);
            }
        }

        /// <summary>
        /// Returns true if window rendering is disabled.
        /// </summary>
        public bool IsWindowRenderingDisabled
        {
            get
            {
                return cef_browser_host_t.is_window_rendering_disabled(_self) != 0;
            }
        }

        /// <summary>
        /// Notify the browser that the widget has been resized. The browser will first
        /// call CefRenderHandler::GetViewRect to get the new size and then call
        /// CefRenderHandler::OnPaint asynchronously with the updated regions. This
        /// method is only used when window rendering is disabled.
        /// </summary>
        public void WasResized()
        {
            cef_browser_host_t.was_resized(_self);
        }

        /// <summary>
        /// Invalidate the |dirtyRect| region of the view. The browser will call
        /// CefRenderHandler::OnPaint asynchronously with the updated regions. This
        /// method is only used when window rendering is disabled.
        /// </summary>
        public void Invalidate(CefRectangle dirtyRect, CefPaintElementType type)
        {
            var n_dirtyRect = new cef_rect_t(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
            cef_browser_host_t.invalidate(_self, &n_dirtyRect, type);
        }

        /// <summary>
        /// Send a key event to the browser.
        /// </summary>
        public void SendKeyEvent(CefKeyEvent keyEvent)
        {
            if (keyEvent == null) throw new ArgumentNullException("keyEvent");

            var n_event = new cef_key_event_t();
            keyEvent.ToNative(&n_event);
            cef_browser_host_t.send_key_event(_self, &n_event);
        }

        /// <summary>
        /// Send a mouse click event to the browser. The |x| and |y| coordinates are
        /// relative to the upper-left corner of the view.
        /// </summary>
        public void SendMouseClickEvent(CefMouseEvent @event, CefMouseButtonType type, bool mouseUp, int clickCount)
        {
            var n_event = @event.ToNative();
            cef_browser_host_t.send_mouse_click_event(_self, &n_event, type, mouseUp ? 1 : 0, clickCount);
        }

        /// <summary>
        /// Send a mouse move event to the browser. The |x| and |y| coordinates are
        /// relative to the upper-left corner of the view.
        /// </summary>
        public void SendMouseMoveEvent(CefMouseEvent @event, bool mouseLeave)
        {
            var n_event = @event.ToNative();
            cef_browser_host_t.send_mouse_move_event(_self, &n_event, mouseLeave ? 1 : 0);
        }

        /// <summary>
        /// Send a mouse wheel event to the browser. The |x| and |y| coordinates are
        /// relative to the upper-left corner of the view. The |deltaX| and |deltaY|
        /// values represent the movement delta in the X and Y directions respectively.
        /// In order to scroll inside select popups with window rendering disabled
        /// CefRenderHandler::GetScreenPoint should be implemented properly.
        /// </summary>
        public void SendMouseWheelEvent(CefMouseEvent @event, int deltaX, int deltaY)
        {
            var n_event = @event.ToNative();
            cef_browser_host_t.send_mouse_wheel_event(_self, &n_event, deltaX, deltaY);
        }

        /// <summary>
        /// Send a focus event to the browser.
        /// </summary>
        public void SendFocusEvent(bool setFocus)
        {
            cef_browser_host_t.send_focus_event(_self, setFocus ? 1 : 0);
        }

        /// <summary>
        /// Send a capture lost event to the browser.
        /// </summary>
        public void SendCaptureLostEvent()
        {
            cef_browser_host_t.send_capture_lost_event(_self);
        }
    }
}
