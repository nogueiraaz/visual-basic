' Classe que representa um Utilizador da biblioteca
Public Class Utilizador
    ' Propriedades do Utilizador
    Public Property Id As Integer
    Public Property Nome As String
    Public Property Turma As String
    Public Property Contacto As String
    Public Property DataCriacao As DateTime
    
    ' Construtor padrão
    Public Sub New()
    End Sub
    
    ' Construtor com parâmetros
    Public Sub New(id As Integer, nome As String, turma As String, contacto As String)
        Me.Id = id
        Me.Nome = nome
        Me.Turma = turma
        Me.Contacto = contacto
    End Sub
End Class