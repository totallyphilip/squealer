Public Class ParameterCollection

    Private _Items As New List(Of ParameterClass)

    Public Sub Add(p As ParameterClass)
        _Items.Add(p)
    End Sub

    Public Function ReturnType() As ParameterClass
        Return _Items.Find(Function(x) x.Name = String.Empty)
    End Function

    Public Function Items() As List(Of ParameterClass)
        Return _Items.FindAll(Function(x) Not String.IsNullOrEmpty(x.Name))
    End Function

End Class

Public Class ParameterClass

    Private _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name.Replace("@", "")
        End Get
    End Property

    Private _Type As String
    Public ReadOnly Property Type As String
        Get
            Dim t As String = _Type
            If t.ToLower.Contains("char") Then
                t &= String.Format("({0})", IIf(_Length = -1, "max", _Length.ToString))
            End If
            Return t
        End Get
    End Property

    Private _Length As Integer
    Public ReadOnly Property Length As Integer
        Get
            Return _Length
        End Get
    End Property

    Private _IsOutput As Boolean
    Public ReadOnly Property IsOutput As Boolean
        Get
            Return _IsOutput
        End Get
    End Property

    Public Sub New(name As String, type As String, length As Integer, isoutput As Boolean)
        _Name = name
        _Type = type
        _Length = length
        _IsOutput = isoutput
    End Sub

End Class
