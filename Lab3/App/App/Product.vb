Public Class Product
    Public Dim Id As Int32
    Public Dim Name As String
    Public Dim Price As Decimal
    Public Dim IsAvailable As Boolean
    
    Public Overrides Function ToString() As String
        Return $"Id: {Id}, Name: {Name}, Price: {Price}, Is available: {IsAvailable}"
    End Function
End Class