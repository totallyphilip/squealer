Public Class FileHash

    Private _Filename As String
    Public ReadOnly Property Filename As String
        Get
            Dim f As String = _Filename.Remove(0, _Filename.LastIndexOf("\") + 1)
            Return f.Remove(f.LastIndexOf(Constants.SquealerFileExtension))
        End Get
    End Property
    Public ReadOnly Property FullFilename As String
        Get
            Return _Filename
        End Get
    End Property

    Private _Hash As String
    Public ReadOnly Property Hash As String
        Get
            Return _Hash
        End Get
    End Property

    Public Sub New(f As String, h As String)
        _Filename = f
        _Hash = h
    End Sub

End Class

Public Class FileHashCollection

    Private _Items As New List(Of FileHash)
    Public ReadOnly Property Items As List(Of FileHash)
        Get
            Return _Items
        End Get
    End Property

    Private _CacheDate As DateTime = DateTime.Now
    Public ReadOnly Property CacheDate As DateTime
        Get
            Return _CacheDate
        End Get
    End Property

    Private _Project As String = "?"
    Public ReadOnly Property Project As String
        Get
            Return _Project
        End Get
    End Property

    Private _Branch As String = "?"
    Public ReadOnly Property Branch As String
        Get
            Return _Branch
        End Get
    End Property

    Public Sub Reset(p As String, b As String)
        _Items.RemoveRange(0, _Items.Count)
        _CacheDate = DateTime.Now
        _Project = p
        _Branch = b
    End Sub

    Public Sub AddItem(f As String)
        _Items.Add(New FileHash(f, HashIt(f)))
    End Sub

    Private Function HashIt(filename As String) As String

        Dim input As System.Security.Cryptography.SHA512 = System.Security.Cryptography.SHA512Managed.Create()
        Dim filebytes As Byte() = My.Computer.FileSystem.ReadAllBytes(filename)
        Dim hash As Byte() = input.ComputeHash(filebytes)

        Dim stringBuilder As New System.Text.StringBuilder()
        For i As Integer = 0 To hash.Length - 1
            stringBuilder.Append(hash(i).ToString("X2"))
        Next

        Return stringBuilder.ToString()

    End Function

    Public Function MatchExists(f As String) As Boolean

        Dim checkhash As New FileHash(f, HashIt(f))

        Return Me.Items.Exists(Function(x) x.Filename = checkhash.Filename And x.Hash = checkhash.Hash)

    End Function

End Class