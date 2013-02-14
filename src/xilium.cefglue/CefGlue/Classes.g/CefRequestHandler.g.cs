//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;
    
    // Role: HANDLER
    public abstract unsafe partial class CefRequestHandler
    {
        private static Dictionary<IntPtr, CefRequestHandler> _roots = new Dictionary<IntPtr, CefRequestHandler>();
        
        private int _refct;
        private cef_request_handler_t* _self;
        
        protected object SyncRoot { get { return this; } }
        
        private cef_request_handler_t.add_ref_delegate _ds0;
        private cef_request_handler_t.release_delegate _ds1;
        private cef_request_handler_t.get_refct_delegate _ds2;
        private cef_request_handler_t.on_before_resource_load_delegate _ds3;
        private cef_request_handler_t.get_resource_handler_delegate _ds4;
        private cef_request_handler_t.on_resource_redirect_delegate _ds5;
        private cef_request_handler_t.get_auth_credentials_delegate _ds6;
        private cef_request_handler_t.on_quota_request_delegate _ds7;
        private cef_request_handler_t.get_cookie_manager_delegate _ds8;
        private cef_request_handler_t.on_protocol_execution_delegate _ds9;
        private cef_request_handler_t.on_before_plugin_load_delegate _dsa;
        
        protected CefRequestHandler()
        {
            _self = cef_request_handler_t.Alloc();
        
            _ds0 = new cef_request_handler_t.add_ref_delegate(add_ref);
            _self->_base._add_ref = Marshal.GetFunctionPointerForDelegate(_ds0);
            _ds1 = new cef_request_handler_t.release_delegate(release);
            _self->_base._release = Marshal.GetFunctionPointerForDelegate(_ds1);
            _ds2 = new cef_request_handler_t.get_refct_delegate(get_refct);
            _self->_base._get_refct = Marshal.GetFunctionPointerForDelegate(_ds2);
            _ds3 = new cef_request_handler_t.on_before_resource_load_delegate(on_before_resource_load);
            _self->_on_before_resource_load = Marshal.GetFunctionPointerForDelegate(_ds3);
            _ds4 = new cef_request_handler_t.get_resource_handler_delegate(get_resource_handler);
            _self->_get_resource_handler = Marshal.GetFunctionPointerForDelegate(_ds4);
            _ds5 = new cef_request_handler_t.on_resource_redirect_delegate(on_resource_redirect);
            _self->_on_resource_redirect = Marshal.GetFunctionPointerForDelegate(_ds5);
            _ds6 = new cef_request_handler_t.get_auth_credentials_delegate(get_auth_credentials);
            _self->_get_auth_credentials = Marshal.GetFunctionPointerForDelegate(_ds6);
            _ds7 = new cef_request_handler_t.on_quota_request_delegate(on_quota_request);
            _self->_on_quota_request = Marshal.GetFunctionPointerForDelegate(_ds7);
            _ds8 = new cef_request_handler_t.get_cookie_manager_delegate(get_cookie_manager);
            _self->_get_cookie_manager = Marshal.GetFunctionPointerForDelegate(_ds8);
            _ds9 = new cef_request_handler_t.on_protocol_execution_delegate(on_protocol_execution);
            _self->_on_protocol_execution = Marshal.GetFunctionPointerForDelegate(_ds9);
            _dsa = new cef_request_handler_t.on_before_plugin_load_delegate(on_before_plugin_load);
            _self->_on_before_plugin_load = Marshal.GetFunctionPointerForDelegate(_dsa);
        }
        
        ~CefRequestHandler()
        {
            Dispose(false);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_self != null)
            {
                cef_request_handler_t.Free(_self);
                _self = null;
            }
        }
        
        private int add_ref(cef_request_handler_t* self)
        {
            lock (SyncRoot)
            {
                var result = ++_refct;
                if (result == 1)
                {
                    lock (_roots) { _roots.Add((IntPtr)_self, this); }
                }
                return result;
            }
        }
        
        private int release(cef_request_handler_t* self)
        {
            lock (SyncRoot)
            {
                var result = --_refct;
                if (result == 0)
                {
                    lock (_roots) { _roots.Remove((IntPtr)_self); }
                }
                return result;
            }
        }
        
        private int get_refct(cef_request_handler_t* self)
        {
            return _refct;
        }
        
        internal cef_request_handler_t* ToNative()
        {
            add_ref(_self);
            return _self;
        }
        
        [Conditional("DEBUG")]
        private void CheckSelf(cef_request_handler_t* self)
        {
            if (_self != self) throw ExceptionBuilder.InvalidSelfReference();
        }
        
    }
}