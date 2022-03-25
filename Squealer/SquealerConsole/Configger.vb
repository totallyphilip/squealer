Namespace My

    ' Beware of case sensitivity! The setting name must always be trimmed and lowercased when referenced in the cache, or null references and unexpected problems will occur.

    '<HideModuleName()> ' No, don't hide it from IntelliSense!
    Module Configger

#Region " General Stuff "

        Private Const SettingFileExtension As String = "setting"

        ' The root folder for all the settings files.
        Private _AppDataFolder As String = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\", My.Application.Info.CompanyName, "\", My.Application.Info.Title)

        ' The calculated filename for a given setting name.
        Private Function SettingFilename(setting As String) As String
            Return SettingFilename(setting, SettingFileExtension)
        End Function
        Private Function SettingFilename(setting As String, ext As String) As String
            Return String.Concat(_AppDataFolder, "\", setting.Trim.ToLower, ".", ext)
        End Function

#End Region

#Region " Cached Settings "

        ' Cache each setting that's saved to or read from disk, thereby reducing disk usage if the settings are read frequently.

#Region " Sub Classes "

        Private Class _CacheStructure

            Private _setting As String
            Friend ReadOnly Property setting As String
                Get
                    Return _setting
                End Get
            End Property

            Private _value As Byte()
            Friend Property value As Byte()
                Get
                    Return _value
                End Get
                Set(value As Byte())
                    _value = value
                End Set
            End Property

            Public Sub New(setting As String, value As Byte())
                _setting = setting.Trim.ToLower
                _value = value
            End Sub

        End Class

#End Region

        Private _Cache As New List(Of _CacheStructure)

        Private Function CacheExists(setting As String) As Boolean
            Return _Cache.Exists(Function(x) x.setting = setting.Trim.ToLower)
        End Function

        Private Function CachedSetting(setting As String) As _CacheStructure
            Return _Cache.Find(Function(x) x.setting = setting.Trim.ToLower)
        End Function

        Private Function CachedValue(setting As String) As Byte()
            Return CachedSetting(setting).value
        End Function

        Private Sub CacheUpdate(setting As String, bytes As Byte())
            Try
                CachedSetting(setting).value = bytes ' This will error if the setting isn't cached.
            Catch ex As Exception
                _Cache.Add(New _CacheStructure(setting, bytes))
            End Try
        End Sub

        ' Delete settings files that were never read (careful! make sure you use them or you'll lose them!); useful to automatically clean the settings directory when the application changes.
        Public Sub PruneSettings()

            For Each FullPathFilename As String In My.Computer.FileSystem.GetFiles(_AppDataFolder, FileIO.SearchOption.SearchTopLevelOnly, String.Concat("*.", SettingFileExtension))
                Try
                    ' Get the name of the setting from the filename.
                    Dim SettingFromFilename As String = My.Computer.FileSystem.GetFileInfo(FullPathFilename).Name.Replace(String.Concat(".", SettingFileExtension), String.Empty)
                    ' Check if we have a cached value for the setting. If not, then assume we never referenced it, and therefore we don't need it anymore.
                    If Not CacheExists(SettingFromFilename) Then

                        ' We can't trap errors and suppress messages when deleting files, so try to move it first, and if the move is successful, deleting it will be successful also.
                        Try
                            Dim MovedFilename As String = String.Concat(FullPathFilename, ".deleteme")
                            With My.Computer.FileSystem
                                .MoveFile(FullPathFilename, MovedFilename)
                                .DeleteFile(MovedFilename, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently) ' This should always work, since we could move it.
                            End With
                        Catch ex As Exception
                            ' Ignore errors; the old file will just sit there for now.
                        End Try

                    End If

                Catch ex As Exception
                    ' Ignore errors.
                End Try
            Next


        End Sub

#End Region

#Region " Save "

        ' Save a byte array (primary overload). Handle caching here.
        Public Sub SaveSetting(setting As String, bytes As Byte())
            If Not My.Computer.FileSystem.DirectoryExists(_AppDataFolder) Then
                My.Computer.FileSystem.CreateDirectory(_AppDataFolder)
            End If
            CacheUpdate(setting, bytes)
            My.Computer.FileSystem.WriteAllBytes(SettingFilename(setting), bytes, False)

            If Not My.Computer.FileSystem.FileExists(SettingFilename("readme!", "txt")) Then
                My.Computer.FileSystem.WriteAllText(SettingFilename("readme!", "txt"), String.Concat("The contents of this folder were generated by ", My.Application.Info.Title, ". Modifying anything here may cause the application to malfunction."), False)
            End If
        End Sub

        ' Save a string.
        Public Sub SaveSetting(setting As String, value As String)
            SaveSetting(setting, System.Text.Encoding.Unicode.GetBytes(value))
        End Sub

        ' Save a datetime.
        Public Sub SaveSetting(setting As String, value As DateTime)
            ' This could be saved directly as a byte array, but let's save it as a string to make the file human-readable.
            SaveSetting(setting, value.ToString)
        End Sub

        ' Save a boolean.
        Public Sub SaveSetting(setting As String, value As Boolean)
            ' This could be saved directly as a byte array, but let's save it as a string to make the file human-readable.
            SaveSetting(setting, value.ToString)
        End Sub

        ' Save a single-precision, floating-point number.
        Public Sub SaveSetting(setting As String, value As Single)
            ' This could be saved directly as a byte array, but let's save it as a string to make the file human-readable.
            SaveSetting(setting, value.ToString)
        End Sub

        ' Save an integer.
        Public Sub SaveSetting(setting As String, value As Integer)
            ' This could be saved directly as a byte array, but let's save it as a string to make the file human-readable.
            SaveSetting(setting, BitConverter.GetBytes(value))
        End Sub

        ' Save a font.
        Public Sub SaveSetting(setting As String, value As System.Drawing.Font)
            SaveSetting(setting, String.Concat(value.Name, ";", value.SizeInPoints.ToString, ";", value.Style.ToString))
        End Sub

#End Region

#Region " Try Load "

        ' Load a byte array (primary overload). Handle caching here.
        Public Function TryLoadSetting(setting As String, ByRef bytes As Byte()) As Boolean
            Try
                Try
                    bytes = CachedValue(setting) ' This will error if the setting isn't cached.
                Catch ex As Exception
                    bytes = My.Computer.FileSystem.ReadAllBytes(SettingFilename(setting))
                    CacheUpdate(setting, bytes)
                End Try
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ' Load a string.
        Public Function TryLoadSetting(setting As String, ByRef value As String) As Boolean

            Dim b As Byte() = Nothing
            If TryLoadSetting(setting, b) Then
                value = System.Text.Encoding.Unicode.GetString(b)
                Return True
            Else
                Return False
            End If

        End Function

        ' Load a datetime.
        Public Function TryLoadSetting(setting As String, ByRef value As DateTime) As Boolean

            Dim s As String = Nothing
            If TryLoadSetting(setting, s) Then
                value = Convert.ToDateTime(s)
                Return True
            Else
                Return False
            End If

        End Function

        ' Load a boolean.
        Public Function TryLoadSetting(setting As String, ByRef value As Boolean) As Boolean

            Dim s As String = Nothing
            If TryLoadSetting(setting, s) Then
                value = Convert.ToBoolean(s)
                Return True
            Else
                Return False
            End If

        End Function

        ' Load a single-precision, floating-point number.
        Public Function TryLoadSetting(setting As String, ByRef value As Single) As Boolean

            Dim s As String = Nothing
            If TryLoadSetting(setting, s) Then
                value = Convert.ToSingle(s)
                Return True
            Else
                Return False
            End If

        End Function

        ' Load an integer.
        Public Function TryLoadSetting(setting As String, ByRef value As Integer) As Boolean

            Dim b As Byte() = Nothing
            If TryLoadSetting(setting, b) Then
                value = BitConverter.ToInt32(b, 0)
                Return True
            Else
                Return False
            End If

        End Function

        ' Load a font.
        'Public Function TryLoadSetting(setting As String, ByRef value As System.Drawing.Font) As Boolean

        '    Dim b As Boolean = False

        '    Dim s As String = Nothing
        '    If TryLoadSetting(setting, s) Then

        '        'Dim font As System.Drawing.Font = Nothing
        '        Dim fontComponents As String() = s.Split(";"c)

        '        Try

        '            Select Case fontComponents.Length
        '                Case 2
        '                    value = New System.Drawing.Font(fontComponents(0), Single.Parse(fontComponents(1)))
        '                    b = True
        '                Case 3
        '                    Dim style As FontStyle = DirectCast([Enum].Parse(GetType(FontStyle), fontComponents(2)), FontStyle)
        '                    value = New System.Drawing.Font(fontComponents(0), Single.Parse(fontComponents(1)), style)
        '                    b = True
        '            End Select

        '        Catch ex As Exception
        '            ' Ignore.
        '        End Try

        '    End If

        '    Return b

        'End Function

#End Region

#Region " Load "

        ' Load a byte array (primary overload).
        Public Function LoadSetting(setting As String, defaultValue As Byte()) As Byte()

            Dim b As Byte() = Nothing
            If TryLoadSetting(setting, b) Then
                Return b
            Else
                CacheUpdate(setting, defaultValue)
                Return defaultValue
            End If

        End Function

        ' Load a string.
        Public Function LoadSetting(setting As String, defaultValue As String) As String

            Dim s As String = Nothing
            If TryLoadSetting(setting, s) Then
                Return s
            Else
                Return defaultValue
            End If

        End Function

        ' Load a datetime.
        Public Function LoadSetting(setting As String, defaultValue As DateTime) As DateTime

            Dim s As DateTime = Nothing
            If TryLoadSetting(setting, s) Then
                Return s
            Else
                Return defaultValue
            End If

        End Function

        ' Load a boolean.
        Public Function LoadSetting(setting As String, defaultValue As Boolean) As Boolean

            Dim s As Boolean = Nothing
            If TryLoadSetting(setting, s) Then
                Return s
            Else
                Return defaultValue
            End If

        End Function

        ' Load a single-precision, floating-point number.
        Public Function LoadSetting(setting As String, defaultValue As Single) As Single

            Dim s As Single = Nothing
            If TryLoadSetting(setting, s) Then
                Return s
            Else
                Return defaultValue
            End If

        End Function

        ' Load an integer.
        Public Function LoadSetting(setting As String, defaultValue As Integer) As Integer

            Dim s As Int32 = Nothing
            If TryLoadSetting(setting, s) Then
                Return s
            Else
                Return defaultValue
            End If

        End Function

        ' Load a font.
        'Public Function LoadSetting(setting As String, defaultValue As System.Drawing.Font) As System.Drawing.Font

        '    Dim f As System.Drawing.Font = Nothing
        '    If TryLoadSetting(setting, f) Then
        '        Return f
        '    Else
        '        Return defaultValue
        '    End If

        'End Function

#End Region

    End Module

End Namespace