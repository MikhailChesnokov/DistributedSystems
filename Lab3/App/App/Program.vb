Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Module Program
    
    Dim ReadOnly BaseAddress = "http://localhost:5000/"
    
    Sub Main()
        
        Dim products = JsonConvert.DeserializeObject(Of IEnumerable(Of Product))(File.ReadAllText("products.json"))

        Write(products)
        
        Dim content = New StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json")
        
        Using client = new HttpClient()
            
            client.BaseAddress = New Uri(BaseAddress)
            
            Using response As HttpResponseMessage = client.PostAsync("Products/CreateOrder", content).GetAwaiter().GetResult()
        
                response.EnsureSuccessStatusCode()
            
                File.WriteAllBytes("result.pdf", response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult())
                
            End Using
        
        End Using
        
    End Sub


    Private Sub Write(products As IEnumerable(Of Product))
        
        products.ToList().ForEach(sub(product) Console.WriteLine(product))
    
    End Sub
    
End Module