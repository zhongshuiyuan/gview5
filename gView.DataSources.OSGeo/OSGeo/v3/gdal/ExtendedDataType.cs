//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace OSGeo_v3.GDAL {

using global::System;
using global::System.Runtime.InteropServices;

public class ExtendedDataType : global::System.IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;
  protected object swigParentRef;

  protected static object ThisOwn_true() { return null; }
  protected object ThisOwn_false() { return this; }

  public ExtendedDataType(IntPtr cPtr, bool cMemoryOwn, object parent) {
    swigCMemOwn = cMemoryOwn;
    swigParentRef = parent;
    swigCPtr = new HandleRef(this, cPtr);
  }

  public static HandleRef getCPtr(ExtendedDataType obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }
  public static HandleRef getCPtrAndDisown(ExtendedDataType obj, object parent) {
    if (obj != null)
    {
      obj.swigCMemOwn = false;
      obj.swigParentRef = parent;
      return obj.swigCPtr;
    }
    else
    {
      return new HandleRef(null, IntPtr.Zero);
    }
  }
  public static HandleRef getCPtrAndSetReference(ExtendedDataType obj, object parent) {
    if (obj != null)
    {
      obj.swigParentRef = parent;
      return obj.swigCPtr;
    }
    else
    {
      return new HandleRef(null, IntPtr.Zero);
    }
  }

  ~ExtendedDataType() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          GdalPINVOKE.delete_ExtendedDataType(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public static ExtendedDataType Create(DataType dt) {
    IntPtr cPtr = GdalPINVOKE.ExtendedDataType_Create((int)dt);
    ExtendedDataType ret = (cPtr == IntPtr.Zero) ? null : new ExtendedDataType(cPtr, true, ThisOwn_true());
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static ExtendedDataType CreateString(uint nMaxStringLength) {
    IntPtr cPtr = GdalPINVOKE.ExtendedDataType_CreateString(nMaxStringLength);
    ExtendedDataType ret = (cPtr == IntPtr.Zero) ? null : new ExtendedDataType(cPtr, true, ThisOwn_true());
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public string GetName() {
    string ret = GdalPINVOKE.ExtendedDataType_GetName(swigCPtr);
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public ExtendedDataTypeClass GetClass() {
    ExtendedDataTypeClass ret = (ExtendedDataTypeClass)GdalPINVOKE.ExtendedDataType_GetClass(swigCPtr);
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public DataType GetNumericDataType() {
    DataType ret = (DataType)GdalPINVOKE.ExtendedDataType_GetNumericDataType(swigCPtr);
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public uint GetSize() {
    uint ret = GdalPINVOKE.ExtendedDataType_GetSize(swigCPtr);
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public uint GetMaxStringLength() {
    uint ret = GdalPINVOKE.ExtendedDataType_GetMaxStringLength(swigCPtr);
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool CanConvertTo(ExtendedDataType other) {
    bool ret = GdalPINVOKE.ExtendedDataType_CanConvertTo(swigCPtr, ExtendedDataType.getCPtr(other));
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool Equals(ExtendedDataType other) {
    bool ret = GdalPINVOKE.ExtendedDataType_Equals(swigCPtr, ExtendedDataType.getCPtr(other));
    if (GdalPINVOKE.SWIGPendingException.Pending) throw GdalPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
