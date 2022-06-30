﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Squealer.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to /***********************************************************************
        '''	Delete deprecated squealer log tables.
        '''***********************************************************************/
        '''
        '''if exists (select 1 from sys.objects where name like &apos;%squealer%&apos;)
        '''	or exists (select 1 from sys.schemas where name = &apos;squealer&apos;)
        '''select 
        '''	s.name
        '''	,o.name
        '''	,o.type_desc
        '''from
        '''	sys.objects o
        '''join
        '''	sys.schemas s
        '''	on s.schema_id = o.schema_id
        '''where
        '''	o.name like &apos;%squealer%&apos;
        '''	or s.name = &apos;squealer&apos;
        '''union
        '''selec [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property _TopScript() As String
            Get
                Return ResourceManager.GetString("_TopScript", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to declare @table_id int = {TableId};
        '''
        '''with ctePkCols as
        '''(
        '''	select
        '''		i.object_id as table_id
        '''		,c.column_id
        '''	from
        '''		sys.indexes i
        '''	join
        '''		sys.index_columns c
        '''		on c.object_id = i.object_id
        '''		and c.index_id = i.index_id
        '''	where
        '''		i.is_primary_key = &apos;true&apos;
        ''')
        ''',cteDefaults as
        '''(
        '''	select
        '''		c.parent_object_id as table_id
        '''		,c.parent_column_id as column_id
        '''		,c.definition
        '''	from
        '''		sys.default_constraints c
        ''')
        '''select
        '''	c.name as [column]
        '''	,ty.name as [type]
        '''	,c.is_identity
        '''	,c.is_rowguidcol
        '''	, [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property AutoGetColumns() As String
            Get
                Return ResourceManager.GetString("AutoGetColumns", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to with cteOperations as
        '''(
        '''	select &apos;Insert&apos; as [operation]
        '''	union select &apos;Update&apos;
        '''	union select &apos;Delete&apos;
        '''	union select &apos;Get&apos;
        '''	union select &apos;List&apos;
        ''')
        '''select
        '''	s.name as [schema]
        '''	,t.name as [table]
        '''	,t.object_id as [table_id]
        '''	,o.Operation
        '''from
        '''	sys.tables t
        '''join
        '''	sys.schemas s
        '''	on s.schema_id = t.schema_id
        '''cross join
        '''	cteOperations o
        '''--where
        '''--	s.name like &apos;%&apos; + @schemafilter + &apos;%&apos;
        '''--	and t.name like &apos;%&apos; + @tablefilter + &apos;%&apos;
        '''order by
        '''	s.name
        '''	,t.name
        '''	,o.Operation
        '''.
        '''</summary>
        Friend ReadOnly Property AutoGetTables() As String
            Get
                Return ResourceManager.GetString("AutoGetTables", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;us-ascii&quot;?&gt;
        '''&lt;Squealer Type=&quot;StoredProcedure&quot;&gt;
        '''&lt;Comments&gt;&lt;![CDATA[{Comments}]]&gt;&lt;/Comments&gt;
        '''&lt;Parameters&gt;{Parameters}&lt;/Parameters&gt;
        '''&lt;Code&gt;&lt;![CDATA[{Code}]]&gt;&lt;/Code&gt;
        '''&lt;Users&gt;{Users}&lt;/Users&gt;
        '''&lt;/Squealer&gt;.
        '''</summary>
        Friend ReadOnly Property AutoProcTemplate() As String
            Get
                Return ResourceManager.GetString("AutoProcTemplate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 1.0.6.0
        '''Added HASH command, and GENERATE -DIFF option.
        '''^break
        '''1.0.5.0
        '''Added error log to AppData folder to assist with troubleshooting.
        '''Title bar and command prompt information is configurable now.
        '''Added option to keep screen alive.
        '''Fixed error reading filenames containing &quot;$&quot; character.
        '''Several minor improvements.
        '''^break
        '''Earlier
        '''This application has been under development since the late &apos;90s,
        '''and has been rewritten from scratch to version 1.0.0.0 several times.
        '''I&apos;m truncating the change log..
        '''</summary>
        Friend ReadOnly Property ChangeLog() As String
            Get
                Return ResourceManager.GetString("ChangeLog", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''/***********************************************************************
        '''
        '''title : {Schema}.{RootProgramName}
        '''{Comments}
        '''***********************************************************************/
        '''.
        '''</summary>
        Friend ReadOnly Property Comment() As String
            Get
                Return ResourceManager.GetString("Comment", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to set nocount on;
        '''
        '''create table #CodeToDrop ([Type] nvarchar(10), [Schema] nvarchar(10), [Name] nvarchar(500));
        '''
        '''{RoutineList}
        '''
        '''with cteObjects as
        '''(
        '''	select
        '''		r.ROUTINE_TYPE as ObjectType
        '''		,r.ROUTINE_SCHEMA as ObjectSchema
        '''		,r.ROUTINE_NAME as ObjectName
        '''	from
        '''		INFORMATION_SCHEMA.ROUTINES r
        '''	union
        '''	select
        '''		&apos;VIEW&apos;
        '''		,v.TABLE_SCHEMA
        '''		,v.TABLE_NAME
        '''	from
        '''		INFORMATION_SCHEMA.VIEWS v
        ''')
        '''select
        '''	o.ObjectType collate database_default as ObjectType
        '''	,o.ObjectSchema collate database_default [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property CompareObjects() As String
            Get
                Return ResourceManager.GetString("CompareObjects", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property DroidScream() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("DroidScream", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''if object_id(&apos;[{Schema}].[{RootProgramName}]&apos;,&apos;p&apos;) is not null
        '''	drop procedure [{Schema}].[{RootProgramName}];
        '''if object_id(&apos;[{Schema}].[{RootProgramName}]&apos;,&apos;fn&apos;) is not null
        '''	drop function [{Schema}].[{RootProgramName}];
        '''if object_id(&apos;[{Schema}].[{RootProgramName}]&apos;,&apos;if&apos;) is not null
        '''	drop function [{Schema}].[{RootProgramName}];
        '''if object_id(&apos;[{Schema}].[{RootProgramName}]&apos;,&apos;tf&apos;) is not null
        '''	drop function [{Schema}].[{RootProgramName}];
        '''if object_id(&apos;[{Schema}].[{RootProgramName}]&apos;,&apos;v&apos;) is not  [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property DropAny() As String
            Get
                Return ResourceManager.GetString("DropAny", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''go
        '''
        '''declare @ezdrop varchar(max) =
        '''(
        '''	select string_agg(
        '''		&apos;drop &apos;
        '''		+case o.type
        '''			when &apos;V&apos; then &apos;view&apos;
        '''			when &apos;P&apos; then &apos;procedure&apos;
        '''			when &apos;FN&apos; then &apos;function&apos;
        '''		end
        '''		+ concat(&apos; [&apos;,s.name,&apos;].[&apos;,o.name,&apos;]&apos;)
        '''			,&apos;; &apos;)
        '''		+ &apos;; drop schema {Schema}&apos;
        '''	from sys.objects o
        '''	join sys.schemas s
        '''		on s.schema_id = o.schema_id
        '''	where s.name = &apos;{Schema}&apos;
        ''')
        '''if @ezdrop is not null
        '''	exec (@ezdrop)
        '''
        '''go
        '''
        '''if schema_id(&apos;{Schema}&apos;) is null
        '''	exec (&apos;create schema {Schema}&apos;)
        '''
        '''go
        '''
        '''create view {Sch [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property EzObjects() As String
            Get
                Return ResourceManager.GetString("EzObjects", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        ''')
        '''
        '''returns {ReturnDataType}
        '''
        '''{WithOptions}
        '''as
        '''begin
        '''set ansi_nulls on;
        '''
        '''declare @Result {ReturnDataType}
        '''
        ''';.
        '''</summary>
        Friend ReadOnly Property FN_Begin() As String
            Get
                Return ResourceManager.GetString("FN_Begin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''set ansi_nulls on;
        '''
        '''declare @Result {ReturnDataType};
        '''
        ''';.
        '''</summary>
        Friend ReadOnly Property FN_BeginTest() As String
            Get
                Return ResourceManager.GetString("FN_BeginTest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''create function [{Schema}].[{RootProgramName}]
        '''(
        '''.
        '''</summary>
        Friend ReadOnly Property FN_Create() As String
            Get
                Return ResourceManager.GetString("FN_Create", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''/***********************************************************************
        '''    Return the function result.
        '''***********************************************************************/
        '''
        '''return @Result
        '''end
        '''.
        '''</summary>
        Friend ReadOnly Property FN_End() As String
            Get
                Return ResourceManager.GetString("FN_End", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''/***********************************************************************
        '''    Return the function result.
        '''***********************************************************************/
        '''
        '''select @Result as [Result]
        '''.
        '''</summary>
        Friend ReadOnly Property FN_EndTest() As String
            Get
                Return ResourceManager.GetString("FN_EndTest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;us-ascii&quot;?&gt;
        '''&lt;Squealer Type=&quot;{RootType}&quot;&gt;
        '''&lt;Parameters&gt;
        '''&lt;!--Parameters--&gt;
        '''&lt;/Parameters&gt;
        '''&lt;Returns Type=&quot;{ReturnDataType}&quot; /&gt;
        '''&lt;Code/&gt;
        '''&lt;Users/&gt;
        '''&lt;/Squealer&gt;
        '''.
        '''</summary>
        Friend ReadOnly Property FN_Template() As String
            Get
                Return ResourceManager.GetString("FN_Template", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property Folder() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Folder", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to grant execute on [{Schema}].[{RootProgramName}] to [{User}];.
        '''</summary>
        Friend ReadOnly Property GrantExecute() As String
            Get
                Return ResourceManager.GetString("GrantExecute", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to grant select on [{Schema}].[{RootProgramName}] to [{User}];.
        '''</summary>
        Friend ReadOnly Property GrantSelect() As String
            Get
                Return ResourceManager.GetString("GrantSelect", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to To join an existing leaderboard, fill in the connection string.
        '''
        '''To create a new leaderboard, find the TOP SECRET script. It&apos;s really hard to find. Then share the connection string with your fellow rebel scum.
        '''
        '''If you do not want to connect to a leaderboard, make sure your connection string is blank. Otherwise, the game will attempt to connect and you will have to wait for it to time out.
        '''
        '''To play the game, type HELP PEWPEW at the command prompt..
        '''</summary>
        Friend ReadOnly Property HowToPlay() As String
            Get
                Return ResourceManager.GetString("HowToPlay", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        ''')
        '''
        '''returns table
        '''
        '''{WithOptions}
        '''as
        '''
        '''return.
        '''</summary>
        Friend ReadOnly Property IF_Begin() As String
            Get
                Return ResourceManager.GetString("IF_Begin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;us-ascii&quot;?&gt;
        '''&lt;Squealer Type=&quot;{RootType}&quot;&gt;
        '''&lt;Parameters&gt;
        '''&lt;!--Parameters--&gt;
        '''&lt;/Parameters&gt;
        '''&lt;Code/&gt;
        '''&lt;Users/&gt;
        '''&lt;/Squealer&gt;
        '''.
        '''</summary>
        Friend ReadOnly Property IF_Template() As String
            Get
                Return ResourceManager.GetString("IF_Template", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to /***********************************************
        '''
        '''	How to create a leaderboard in SQL Server!
        '''
        '''	INSTRUCTIONS (THIS IS THE WAY):
        '''	
        '''	Execute this script on a database of your choosing.
        '''
        '''	Upon execution, three things will be created:
        '''	1. dbo.Leaderboard (table)
        '''	2. dbo.LeaderboardAdd (stored procedure)
        '''	3. dbo.LeaderboardRead (stored procedure)
        '''
        '''	Make sure all players have permission to execute 
        '''	the two stored procedures, and share your
        '''	connection string with them!
        '''	
        '''	NOTE: If these objects  [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property LeaderboardCreate() As String
            Get
                Return ResourceManager.GetString("LeaderboardCreate", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to select
        '''	concat(s.name,&apos;.&apos;,o.name) as object_name
        '''	,o.type
        '''	,m.definition
        '''	,o.object_id
        '''from
        '''	sys.objects o
        '''join
        '''	sys.sql_modules m
        '''	on m.object_id = o.object_id
        '''join
        '''	sys.schemas s
        '''	on s.schema_id = o.schema_id
        '''where
        '''	o.type in (&apos;P&apos;,&apos;FN&apos;,&apos;TF&apos;,&apos;IF&apos;,&apos;V&apos;)
        '''order by
        '''	s.name
        '''	,o.name.
        '''</summary>
        Friend ReadOnly Property ObjectList() As String
            Get
                Return ResourceManager.GetString("ObjectList", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to select
        '''	p.name parameter_name
        '''	,t.name type_name
        '''	,p.is_output
        '''	,p.max_length
        '''from
        '''	sys.parameters p
        '''join
        '''	sys.types t
        '''	on t.system_type_id = p.system_type_id
        '''where
        '''	p.object_id = @ObjectId
        '''order by
        '''	p.parameter_id.
        '''</summary>
        Friend ReadOnly Property ObjectParameters() As String
            Get
                Return ResourceManager.GetString("ObjectParameters", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to select
        '''--	concat(&apos;[&apos;,s.name,&apos;].[&apos;,o.name,&apos;]&apos;) as object_name
        '''	u.name user_name
        '''from
        '''	sys.objects o
        '''join
        '''	sys.schemas s
        '''	on s.schema_id = o.schema_id
        '''join
        '''	sys.syspermissions p
        '''	on p.id = o.object_id
        '''join
        '''	sys.sysusers u
        '''	on u.uid = p.grantee
        '''where
        '''	o.object_id = @ObjectId.
        '''</summary>
        Friend ReadOnly Property ObjectPermissions() As String
            Get
                Return ResourceManager.GetString("ObjectPermissions", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''/***********************************************************************
        '''	Begin the transaction. Start the TRY..CATCH wrapper.
        '''***********************************************************************/
        '''
        '''              -- !!!!!  DO NOT EDIT THIS SECTION  !!!!! --
        '''
        '''{WithOptions}
        '''as
        '''set ansi_nulls on;
        '''set nocount on;
        '''set quoted_identifier on;
        '''
        '''declare @Squealer_ReturnValue int = 0;
        '''declare @SqlrInternalErrorNumber int; -- for backward compatibility with pre-release squealer
        '''
        '''begin try
        '''	begin tra [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property P_Begin() As String
            Get
                Return ResourceManager.GetString("P_Begin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''
        '''set ansi_nulls on;
        '''set quoted_identifier on;
        '''
        '''declare @SqlrInternalErrorNumber int = 0; -- Not used in test scripts, but declared to avoid errors.
        '''
        '''begin transaction
        '''
        '''/*######################################################################
        '''                         YOUR CODE BEGINS HERE.
        '''######################################################################*/
        ''';.
        '''</summary>
        Friend ReadOnly Property P_BeginTest() As String
            Get
                Return ResourceManager.GetString("P_BeginTest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''create procedure [{Schema}].[{RootProgramName}]
        '''.
        '''</summary>
        Friend ReadOnly Property P_Create() As String
            Get
                Return ResourceManager.GetString("P_Create", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''/*######################################################################
        '''                          YOUR CODE ENDS HERE.
        '''######################################################################*/
        '''
        '''/***********************************************************************
        '''	Commit the transaction. If we are in a nested transaction, this
        '''	decrements the transaction count.
        '''***********************************************************************/
        '''
        '''              -- !!!!!  DO NOT EDIT THIS SECTION  !!!!! --
        ''' [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property P_End() As String
            Get
                Return ResourceManager.GetString("P_End", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''/*######################################################################
        '''                          YOUR CODE ENDS HERE.
        '''######################################################################*/
        '''
        '''-- This script defaults to ROLLBACK so you can repeat your testing.
        '''
        '''rollback transaction
        '''--commit transaction
        '''
        '''print &apos;@SqlrInternalErrorNumber = &apos; + convert(varchar,(@SqlrInternalErrorNumber));
        '''.
        '''</summary>
        Friend ReadOnly Property P_EndTest() As String
            Get
                Return ResourceManager.GetString("P_EndTest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 		set @Squealer_ErrorMessage =
        '''			@Squealer_ErrorMessage
        '''			+ char(10)
        '''			+ &apos;@{ErrorParameterName} = &apos;
        '''			+ isnull(convert(varchar(max),@{ErrorParameterName}),&apos;[NULL]&apos;);.
        '''</summary>
        Friend ReadOnly Property P_ErrorParameter() As String
            Get
                Return ResourceManager.GetString("P_ErrorParameter", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;us-ascii&quot;?&gt;
        '''&lt;Squealer Type=&quot;{RootType}&quot;&gt;
        '''&lt;Parameters&gt;
        '''&lt;!--Parameters--&gt;
        '''&lt;/Parameters&gt;
        '''&lt;Code/&gt;
        '''&lt;Users/&gt;
        '''&lt;/Squealer&gt;
        '''.
        '''</summary>
        Friend ReadOnly Property P_Template() As String
            Get
                Return ResourceManager.GetString("P_Template", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        '''</summary>
        Friend ReadOnly Property PigNose() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("PigNose", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property RebelAlliance() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("RebelAlliance", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        ''')
        '''
        '''{WithOptions}
        '''as
        '''begin
        '''set ansi_nulls on;
        '''.
        '''</summary>
        Friend ReadOnly Property Tf_Begin() As String
            Get
                Return ResourceManager.GetString("Tf_Begin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        ''')
        '''
        '''set ansi_nulls on;
        '''.
        '''</summary>
        Friend ReadOnly Property Tf_BeginTest() As String
            Get
                Return ResourceManager.GetString("Tf_BeginTest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''return
        '''end.
        '''</summary>
        Friend ReadOnly Property TF_End() As String
            Get
                Return ResourceManager.GetString("TF_End", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''select * from @TableValue;.
        '''</summary>
        Friend ReadOnly Property TF_EndTest() As String
            Get
                Return ResourceManager.GetString("TF_EndTest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        ''')
        '''
        '''returns @TableValue table
        '''(
        '''.
        '''</summary>
        Friend ReadOnly Property TF_Table() As String
            Get
                Return ResourceManager.GetString("TF_Table", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''
        '''declare @TableValue table
        '''(
        '''.
        '''</summary>
        Friend ReadOnly Property TF_TableTest() As String
            Get
                Return ResourceManager.GetString("TF_TableTest", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;us-ascii&quot;?&gt;
        '''&lt;Squealer Type=&quot;{RootType}&quot;&gt;
        '''&lt;Parameters&gt;
        '''&lt;!--Parameters--&gt;
        '''&lt;/Parameters&gt;
        '''&lt;Code/&gt;
        '''&lt;Users/&gt;
        '''&lt;/Squealer&gt;
        '''.
        '''</summary>
        Friend ReadOnly Property TF_Template() As String
            Get
                Return ResourceManager.GetString("TF_Template", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;us-ascii&quot;?&gt;
        '''&lt;!-- Project name maximum 10 characters (longer names will be truncated). --&gt;
        '''&lt;Settings ProjectName=&quot;myproject&quot;&gt;
        '''	&lt;DefaultUsers&gt;
        '''		&lt;User Name=&quot;User1&quot; /&gt;
        '''		&lt;User Name=&quot;User2&quot; /&gt;
        '''	&lt;/DefaultUsers&gt;
        '''&lt;!-- String replacements are case-sensitive and applied sequentially. Format &quot;$TEXT$&quot; is not required but recommended as a visual cue. --&gt;
        '''	&lt;StringReplacements&gt;
        '''		&lt;String Original=&quot;$YODA-QUOTE$&quot; Replacement=&quot;$AFFIRMATIVE$ or $NEGATORY$. There is no Try.&quot; Comment=&quot;&quot; / [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property UserConfig() As String
            Get
                Return ResourceManager.GetString("UserConfig", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''{WithOptions}
        '''as
        '''.
        '''</summary>
        Friend ReadOnly Property V_Begin() As String
            Get
                Return ResourceManager.GetString("V_Begin", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 
        '''create view [{Schema}].[{RootProgramName}]
        '''.
        '''</summary>
        Friend ReadOnly Property V_Create() As String
            Get
                Return ResourceManager.GetString("V_Create", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;us-ascii&quot;?&gt;
        '''&lt;Squealer Type=&quot;{RootType}&quot;&gt;
        '''&lt;Code/&gt;
        '''&lt;Users/&gt;
        '''&lt;/Squealer&gt;
        '''.
        '''</summary>
        Friend ReadOnly Property V_Template() As String
            Get
                Return ResourceManager.GetString("V_Template", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
