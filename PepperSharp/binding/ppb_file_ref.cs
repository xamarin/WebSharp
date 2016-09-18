/* Copyright (c) 2016 Xamarin. */

/* NOTE: this is auto-generated from IDL */
/* From ppb_file_ref.idl modified Thu May 12 07:00:02 2016. */

using System;
using System.Runtime.InteropServices;

namespace PepperSharp {


/**
 * @file
 * This file defines the API to create a file reference or "weak pointer" to a
 * file in a file system.
 */


/**
 * @addtogroup Enums
 * @{
 */
/**
 * The <code>PP_MakeDirectoryFlags</code> enum contains flags used to control
 * behavior of <code>PPB_FileRef.MakeDirectory()</code>.
 */
public enum PPMakeDirectoryFlags {
  MakedirectoryflagNone = 0<<0,
  /** Requests that ancestor directories are created if they do not exist. */
  MakedirectoryflagWithAncestors = 1<<0,
  /**
   * Requests that the PPB_FileRef.MakeDirectory() call fails if the directory
   * already exists.
   */
  MakedirectoryflagExclusive = 1<<1
}
/**
 * @}
 */

/**
 * @addtogroup Interfaces
 * @{
 */
/**
 * The <code>PPB_FileRef</code> struct represents a "weak pointer" to a file in
 * a file system.  This struct contains a <code>PP_FileSystemType</code>
 * identifier and a file path string.
 */
internal static partial class PPBFileRef {
  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_Create")]
  extern static PPResource _Create ( PPResource file_system, IntPtr path);

  /**
   * Create() creates a weak pointer to a file in the given file system. File
   * paths are POSIX style.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a file
   * system.
   * @param[in] path A path to the file. Must begin with a '/' character.
   *
   * @return A <code>PP_Resource</code> corresponding to a file reference if
   * successful or 0 if the path is malformed.
   */
  public static PPResource Create ( PPResource file_system, byte[] path)
  {
  	if (path == null)
  		throw new ArgumentNullException ("path");

  	unsafe
  	{
  		fixed (byte* path_ = &path[0])
  		{
  			return _Create (file_system, (IntPtr) path_);
  		}
  	}
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_IsFileRef")]
  extern static PPBool _IsFileRef ( PPResource resource);

  /**
   * IsFileRef() determines if the provided resource is a file reference.
   *
   * @param[in] resource A <code>PP_Resource</code> corresponding to a file
   * reference.
   *
   * @return <code>PP_TRUE</code> if the resource is a
   * <code>PPB_FileRef</code>, <code>PP_FALSE</code> if the resource is
   * invalid or some type other than <code>PPB_FileRef</code>.
   */
  public static PPBool IsFileRef ( PPResource resource)
  {
  	return _IsFileRef (resource);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_GetFileSystemType")]
  extern static PPFileSystemType _GetFileSystemType ( PPResource file_ref);

  /**
   * GetFileSystemType() returns the type of the file system.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   *
   * @return A <code>PP_FileSystemType</code> with the file system type if
   * valid or <code>PP_FILESYSTEMTYPE_INVALID</code> if the provided resource
   * is not a valid file reference.
   */
  public static PPFileSystemType GetFileSystemType ( PPResource file_ref)
  {
  	return _GetFileSystemType (file_ref);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_GetName")]
  extern static PPVar _GetName ( PPResource file_ref);

  /**
   * GetName() returns the name of the file.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   *
   * @return A <code>PP_Var</code> containing the name of the file.  The value
   * returned by this function does not include any path components (such as
   * the name of the parent directory, for example). It is just the name of the
   * file. Use GetPath() to get the full file path.
   */
  public static PPVar GetName ( PPResource file_ref)
  {
  	return _GetName (file_ref);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_GetPath")]
  extern static PPVar _GetPath ( PPResource file_ref);

  /**
   * GetPath() returns the absolute path of the file.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   *
   * @return A <code>PP_Var</code> containing the absolute path of the file.
   * This function fails if the file system type is
   * <code>PP_FileSystemType_External</code>.
   */
  public static PPVar GetPath ( PPResource file_ref)
  {
  	return _GetPath (file_ref);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_GetParent")]
  extern static PPResource _GetParent ( PPResource file_ref);

  /**
   * GetParent() returns the parent directory of this file.  If
   * <code>file_ref</code> points to the root of the filesystem, then the root
   * is returned.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   *
   * @return A <code>PP_Resource</code> containing the parent directory of the
   * file. This function fails if the file system type is
   * <code>PP_FileSystemType_External</code>.
   */
  public static PPResource GetParent ( PPResource file_ref)
  {
  	return _GetParent (file_ref);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_MakeDirectory")]
  extern static int _MakeDirectory ( PPResource directory_ref,
                                     int make_directory_flags,
                                     PPCompletionCallback callback);

  /**
   * MakeDirectory() makes a new directory in the file system according to the
   * given <code>make_directory_flags</code>, which is a bit-mask of the
   * <code>PP_MakeDirectoryFlags</code> values.  It is not valid to make a
   * directory in the external file system.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   * @param[in] make_directory_flags A bit-mask of the
   * <code>PP_MakeDirectoryFlags</code> values.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of MakeDirectory().
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int MakeDirectory ( PPResource directory_ref,
                                    int make_directory_flags,
                                    PPCompletionCallback callback)
  {
  	return _MakeDirectory (directory_ref, make_directory_flags, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_Touch")]
  extern static int _Touch ( PPResource file_ref,
                             double last_access_time,
                             double last_modified_time,
                             PPCompletionCallback callback);

  /**
   * Touch() Updates time stamps for a file.  You must have write access to the
   * file if it exists in the external filesystem.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   * @param[in] last_access_time The last time the file was accessed.
   * @param[in] last_modified_time The last time the file was modified.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of Touch().
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int Touch ( PPResource file_ref,
                            double last_access_time,
                            double last_modified_time,
                            PPCompletionCallback callback)
  {
  	return _Touch (file_ref, last_access_time, last_modified_time, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_Delete")]
  extern static int _Delete ( PPResource file_ref,
                              PPCompletionCallback callback);

  /**
   * Delete() deletes a file or directory. If <code>file_ref</code> refers to
   * a directory, then the directory must be empty. It is an error to delete a
   * file or directory that is in use.  It is not valid to delete a file in
   * the external file system.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of Delete().
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int Delete ( PPResource file_ref,
                             PPCompletionCallback callback)
  {
  	return _Delete (file_ref, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_Rename")]
  extern static int _Rename ( PPResource file_ref,
                              PPResource new_file_ref,
                              PPCompletionCallback callback);

  /**
   * Rename() renames a file or directory.  Arguments <code>file_ref</code> and
   * <code>new_file_ref</code> must both refer to files in the same file
   * system. It is an error to rename a file or directory that is in use.  It
   * is not valid to rename a file in the external file system.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   * @param[in] new_file_ref A <code>PP_Resource</code> corresponding to a new
   * file reference.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of Rename().
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int Rename ( PPResource file_ref,
                             PPResource new_file_ref,
                             PPCompletionCallback callback)
  {
  	return _Rename (file_ref, new_file_ref, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_Query")]
  extern static int _Query ( PPResource file_ref,
                            out PPFileInfo info,
                             PPCompletionCallback callback);

  /**
   * Query() queries info about a file or directory. You must have access to
   * read this file or directory if it exists in the external filesystem.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a file
   * reference.
   * @param[out] info A pointer to a <code>PP_FileInfo</code> which will be
   * populated with information about the file or directory.
   * @param[in] callback A <code>PP_CompletionCallback</code> to be called upon
   * completion of Query().
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int Query ( PPResource file_ref,
                           out PPFileInfo info,
                            PPCompletionCallback callback)
  {
  	return _Query (file_ref, out info, callback);
  }


  [DllImport("PepperPlugin", EntryPoint = "PPB_FileRef_ReadDirectoryEntries")]
  extern static int _ReadDirectoryEntries ( PPResource file_ref,
                                            PPArrayOutput output,
                                            PPCompletionCallback callback);

  /**
   * ReadDirectoryEntries() reads all entries in a directory.
   *
   * @param[in] file_ref A <code>PP_Resource</code> corresponding to a directory
   * reference.
   * @param[in] output An output array which will receive
   * <code>PP_DirectoryEntry</code> objects on success.
   * @param[in] callback A <code>PP_CompletionCallback</code> to run on
   * completion.
   *
   * @return An int32_t containing an error code from <code>pp_errors.h</code>.
   */
  public static int ReadDirectoryEntries ( PPResource file_ref,
                                           PPArrayOutput output,
                                           PPCompletionCallback callback)
  {
  	return _ReadDirectoryEntries (file_ref, output, callback);
  }


}
/**
 * @}
 */


}
