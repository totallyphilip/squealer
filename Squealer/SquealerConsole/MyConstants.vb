Public Class MyConstants
    Public Shared ReadOnly Property ObjectFileExtension As String
        Get
            Return ".sqlr"
        End Get
    End Property

    Public Shared ReadOnly Property ConfigFilename As String
        Get
            Return My.Application.Info.ProductName & ".config"
        End Get
    End Property

    Public Shared ReadOnly Property ConnectionStringFilename As String
        Get
            Return ".connectionstring"
        End Get
    End Property

    Public Shared ReadOnly Property AutocreateFilename As String
        Get
            Return String.Format("({0})", My.Application.Info.ProductName)
        End Get
    End Property

End Class