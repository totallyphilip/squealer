Public Class SquealerObjectType

    Public Enum eType
        StoredProcedure
        ScalarFunction
        InlineTableFunction
        MultiStatementTableFunction
        View
        Invalid
    End Enum

    Public Enum eShortType
        p ' stored procedure
        fn ' scalar function
        [if] ' inline table function
        tf ' multistatement table function
        v ' view
        err
    End Enum

    Private _SquealerObjectType As eType
    Public Property LongType As eType
        Get
            Return _SquealerObjectType
        End Get
        Set(value As eType)
            _SquealerObjectType = value
        End Set
    End Property
    Public ReadOnly Property ShortType As eShortType
        Get
            Return ToShortType(_SquealerObjectType)
        End Get
    End Property

    Public ReadOnly Property GeneralType As String
        Get
            Select Case _SquealerObjectType
                Case eType.StoredProcedure
                    Return "PROCEDURE"
                Case eType.ScalarFunction, eType.InlineTableFunction, eType.MultiStatementTableFunction
                    Return "FUNCTION"
                Case eType.View
                    Return "VIEW"
                Case Else
                    Return "ERROR"
            End Select
        End Get
    End Property

    Private _Selected As Boolean
    Public Property Selected As Boolean
        Get
            Return _Selected
        End Get
        Set(value As Boolean)
            _Selected = value
        End Set
    End Property

    Sub New()
        Me.New(eType.Invalid, False)
    End Sub
    Sub New(t As eType)
        Me.New(t, False)
    End Sub
    Sub New(t As eType, selected As Boolean)
        _SquealerObjectType = t
        _Selected = selected
    End Sub

    Public Sub SetType(s As String)
        _SquealerObjectType = Eval(s)
    End Sub

    Public ReadOnly Property FriendlyType As String
        Get
            Select Case _SquealerObjectType
                Case eType.StoredProcedure
                    Return "Stored Procedure"
                Case eType.ScalarFunction
                    Return "Scalar Function"
                Case eType.InlineTableFunction
                    Return "Inline Table-Valued Function"
                Case eType.MultiStatementTableFunction
                    Return "Multi-Statement Table-Valued Function"
                Case eType.View
                    Return "View"
                Case Else
                    Return "Invalid"
            End Select
        End Get
    End Property

    Public Shared Function ToShortType(t As SquealerObjectType.eType) As eShortType
        Select Case t
            Case eType.StoredProcedure
                Return eShortType.p
            Case eType.ScalarFunction
                Return eShortType.fn
            Case eType.InlineTableFunction
                Return eShortType.if
            Case eType.MultiStatementTableFunction
                Return eShortType.tf
            Case eType.View
                Return eShortType.v
            Case Else
                Return eShortType.err
        End Select

    End Function

    ' See if the string type matches an enum
    Public Shared Function Validated(ByVal s As String) As Boolean

        Try
            Dim t As eType = DirectCast([Enum].Parse(GetType(eType), s), eType)
            Return True
        Catch tex As Exception
            Try
                Dim st As eShortType = DirectCast([Enum].Parse(GetType(eShortType), s), eShortType)
                Return True
            Catch stex As Exception
                Return False
            End Try
        End Try

    End Function

    Public Shared Function Eval(ByVal s As String) As eType

        Dim t As eType = Nothing

        Try

            ' First try to parse the string as the main file type.
            t = DirectCast([Enum].Parse(GetType(eType), s), eType)

        Catch tex As Exception

            ' If that failed, try to parse the string as the short file type.
            Dim st As eShortType = Nothing

            Try
                st = DirectCast([Enum].Parse(GetType(eShortType), s), eShortType)
            Catch stex As Exception
                st = eShortType.err
            End Try

            Select Case st
                Case eShortType.p
                    t = eType.StoredProcedure
                Case eShortType.fn
                    t = eType.ScalarFunction
                Case eShortType.if
                    t = eType.InlineTableFunction
                Case eShortType.tf
                    t = eType.MultiStatementTableFunction
                Case eShortType.v
                    t = eType.View
                Case Else
                    t = eType.Invalid
            End Select

        End Try

        Eval = t

    End Function

End Class

Public Class SquealerObjectTypeCollection

    Private _Items As New List(Of SquealerObjectType)
    Public ReadOnly Property Items As List(Of SquealerObjectType)
        Get
            Return _Items
        End Get
    End Property

    Public ReadOnly Property AllSelected As Boolean
        Get
            Return _Items.FindAll(Function(x) x.Selected).Count = 6
        End Get
    End Property
    Public ReadOnly Property NoneSelected As Boolean
        Get
            Return _Items.FindAll(Function(x) x.Selected).Count = 0
        End Get
    End Property
    Public ReadOnly Property SelectedCount As Integer
        Get
            Return _Items.FindAll(Function(x) x.Selected).Count
        End Get
    End Property


    Public Sub New()
        With _Items
            .Add(New SquealerObjectType(SquealerObjectType.eType.StoredProcedure))
            .Add(New SquealerObjectType(SquealerObjectType.eType.ScalarFunction))
            .Add(New SquealerObjectType(SquealerObjectType.eType.InlineTableFunction))
            .Add(New SquealerObjectType(SquealerObjectType.eType.MultiStatementTableFunction))
            .Add(New SquealerObjectType(SquealerObjectType.eType.View))
            .Add(New SquealerObjectType(SquealerObjectType.eType.Invalid))
        End With
    End Sub
    Public Sub SetAllFlags(b As Boolean)
        For Each o As SquealerObjectType In _Items
            o.Selected = b
        Next
    End Sub
    Public Sub SetOneFlag(t As SquealerObjectType.eType, b As Boolean)
        _Items.Find(Function(x) x.LongType = t).Selected = b
    End Sub

    Public Function ObjectTypesOptionString(includeinvalid As Boolean) As String
        Dim s As String = String.Empty
        Dim separator As String = String.Empty
        For Each t As SquealerObjectType In _Items.FindAll(Function(x) includeinvalid OrElse x.LongType <> SquealerObjectType.eType.Invalid)
            s = s & String.Format("{0}{1};{2}", separator, t.ShortType.ToString, t.FriendlyType.ToLower)
            separator = "|"
        Next
        Return s
    End Function
End Class

Public Class SquealerObject

    Private _Type As New SquealerObjectType
    Public Property Type As SquealerObjectType
        Get
            Return _Type
        End Get
        Set(value As SquealerObjectType)
            _Type = value
        End Set
    End Property

    Private _Flags As String
    Public Property Flags As String
        Get
            Return _Flags
        End Get
        Set(value As String)
            _Flags = value
        End Set
    End Property

    Private _WithOptions As String
    Public Property WithOptions As String
        Get
            Return _WithOptions
        End Get
        Set(value As String)
            _WithOptions = value
        End Set
    End Property

    Private _RunLog As Boolean
    Public Property RunLog As Boolean
        Get
            Return _RunLog
        End Get
        Set(value As Boolean)
            _RunLog = value
        End Set
    End Property

    Public ReadOnly Property FlagsList As List(Of String)
        Get
            Dim flags As New List(Of String)
            flags.AddRange(_Flags.Split((New Char() {"|"c})))
            flags.RemoveAll(Function(x) String.IsNullOrWhiteSpace(x))
            flags.Sort()
            Return flags
        End Get
    End Property


    Public ReadOnly Property FlagsSummary As String
        Get
            Dim flags As String = String.Empty
            For Each flag As String In Me.FlagsList
                flags &= flag.Split((New Char() {";"c}))(0)
            Next
            Dim plus As Boolean = flags.Length > 5
            Return (flags & "          ").Substring(0, 5) & IIf(plus, "+", " ").ToString
        End Get
    End Property

    Public Sub New(fn As String)

        _Type.LongType = SquealerObjectType.eType.Invalid
        _Flags = String.Empty
        _WithOptions = String.Empty
        _RunLog = False

        Dim Reader As New Xml.XmlDocument

        Try
            Reader.Load(fn)
        Catch ex As Exception
            Reader.LoadXml("<Nope/>")
        End Try

        Dim Node As Xml.XmlNode = Reader.SelectSingleNode("/" & My.Application.Info.ProductName)
        Try
            _Type.LongType = SquealerObjectType.Eval(Node.Attributes("Type").Value.ToString)
        Catch ex As Exception
        End Try

        Try
            _Flags = Node.Attributes("Flags").Value.ToString
        Catch ex As Exception
        End Try

        Try
            _WithOptions = Node.Attributes("WithOptions").Value.ToString
        Catch ex As Exception
        End Try

        Try
            _RunLog = CBool(Node.Attributes("RunLog").Value)
        Catch ex As Exception
        End Try

    End Sub

End Class