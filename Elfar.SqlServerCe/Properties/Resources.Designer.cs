﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Elfar.SqlServerCe.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Elfar.SqlServerCe.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE NONCLUSTERED INDEX [IX_Errors] ON [Errors] 
        ///(
        ///    [Application]	ASC,
        ///    [Sequence]		DESC
        ///).
        /// </summary>
        internal static string Index {
            get {
                return ResourceManager.GetString("Index", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [Application],[Code],[Host],[ID],[Message],[Source],[Time],[Type],[User]
        ///FROM Errors
        ///WHERE [Application] = @Application OR (@Application IS NULL AND [Application] IS NULL)
        ///ORDER BY [Sequence] DESC
        ///OFFSET @Page * @Size ROWS FETCH NEXT @Size ROWS ONLY.
        /// </summary>
        internal static string List {
            get {
                return ResourceManager.GetString("List", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE [Errors]
        ///(
        ///    [Application]		NVARCHAR(60),
        ///    [Code]			    INT,
        ///    [Cookies]			NTEXT,
        ///    [Detail]			NTEXT NOT NULL,
        ///    [Form]			    NTEXT,
        ///    [Host]			    NVARCHAR(50) NOT NULL,
        ///    [Html]			    NTEXT,
        ///    [ID]				UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        ///    [Message]			NTEXT NOT NULL,
        ///    [QueryString]		NTEXT,
        ///    [Sequence]		    INT IDENTITY (0, 1) NOT NULL,
        ///    [ServerVariables]	NTEXT,
        ///    [Source]			NVARCHAR(60) NOT NULL,
        ///    [Time]			    DATETIME NOT NULL [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Table {
            get {
                return ResourceManager.GetString("Table", resourceCulture);
            }
        }
    }
}
