' Classe que representa um Empréstimo de livro
Public Class Emprestimo
    ' Propriedades do Empréstimo
    Public Property Id As Integer
    Public Property IdLivro As Integer
    Public Property IdUtilizador As Integer
    Public Property DataEmprestimo As Date
    Public Property DataDevolucao As Date
    Public Property Status As String ' Ativo / Devolvido
    
    ' Construtor padrão
    Public Sub New()
    End Sub
    
    ' Construtor com parâmetros
    Public Sub New(id As Integer, idLivro As Integer, idUtilizador As Integer, 
                   dataEmprestimo As Date, dataDevolucao As Date, status As String)
        Me.Id = id
        Me.IdLivro = idLivro
        Me.IdUtilizador = idUtilizador
        Me.DataEmprestimo = dataEmprestimo
        Me.DataDevolucao = dataDevolucao
        Me.Status = status
    End Sub
End Class