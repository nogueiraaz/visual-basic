' Classe que representa um Livro da biblioteca
Public Class Livro
    ' Propriedades do Livro
    Public Property Id As Integer
    Public Property Titulo As String
    Public Property Autor As String
    Public Property Ano As Integer
    Public Property Estado As String ' Disponível / Emprestado
    
    ' Construtor padrão
    Public Sub New()
    End Sub
    
    ' Construtor com parâmetros
    Public Sub New(id As Integer, titulo As String, autor As String, ano As Integer, estado As String)
        Me.Id = id
        Me.Titulo = titulo
        Me.Autor = autor
        Me.Ano = ano
        Me.Estado = estado
    End Sub
End Class