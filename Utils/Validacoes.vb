' Classe com métodos de validação de dados
Public Class Validacoes
    
    ' Validar se um campo é obrigatório (não vazio)
    Public Shared Function ValidarCampoObrigatorio(texto As String) As Boolean
        Return Not String.IsNullOrWhiteSpace(texto)
    End Function
    
    ' Validar se uma data é válida
    Public Shared Function ValidarData(data As String) As Boolean
        Dim dataTemp As Date
        Return Date.TryParse(data, dataTemp)
    End Function
    
    ' Validar formato de contacto (apenas números)
    Public Shared Function ValidarContacto(contacto As String) As Boolean
        If String.IsNullOrWhiteSpace(contacto) Then
            Return False
        End If
        Return contacto.All(Function(c) Char.IsDigit(c))
    End Function
    
    ' Validar se um ano é válido
    Public Shared Function ValidarAno(ano As String) As Boolean
        Dim anoTemp As Integer
        If Integer.TryParse(ano, anoTemp) Then
            Return anoTemp >= 1900 AndAlso anoTemp <= Date.Now.Year
        End If
        Return False
    End Function
    
    ' Validar um formulário de Utilizador
    Public Shared Function ValidarUtilizador(nome As String, turma As String, contacto As String) As String
        If Not ValidarCampoObrigatorio(nome) Then
            Return "Nome é obrigatório"
        End If
        If Not ValidarCampoObrigatorio(turma) Then
            Return "Turma é obrigatória"
        End If
        If Not ValidarCampoObrigatorio(contacto) Then
            Return "Contacto é obrigatório"
        End If
        If Not ValidarContacto(contacto) Then
            Return "Contacto inválido (apenas números)"
        End If
        Return ""
    End Function
    
    ' Validar um formulário de Livro
    Public Shared Function ValidarLivro(titulo As String, autor As String, ano As String) As String
        If Not ValidarCampoObrigatorio(titulo) Then
            Return "Título é obrigatório"
        End If
        If Not ValidarCampoObrigatorio(autor) Then
            Return "Autor é obrigatório"
        End If
        If Not ValidarAno(ano) Then
            Return "Ano inválido"
        End If
        Return ""
    End Function
End Class