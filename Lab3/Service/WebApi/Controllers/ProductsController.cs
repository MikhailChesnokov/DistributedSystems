namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DinkToPdf;
    using DinkToPdf.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    
    
    public class ProductsController : ControllerBase
    {
        private readonly IConverter _converter;

        
        
        public ProductsController(IConverter converter)
        {
            _converter = converter;
        }

        
        
        [HttpPost]
        public FileResult CreateOrder([FromBody] IEnumerable<Product> products)
        {
            var file = _converter.Convert(new HtmlToPdfDocument
            {
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent = CreateHtmlMarkup(products.ToList())
                    }
                }
            });

            return File(file, "application/pdf", "result.pdf");
        }


        
        private string CreateHtmlMarkup(IReadOnlyList<Product> products)
        {
            return /*lang=html*/
                @"<html>
                    <head>
                    </head>
                    <body>
                        <h3>Order</h3>
                        <table>
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Name</th>
                                    <th>Price</th>
                                    <th>Is Available</th>                                       
                                </tr>
                            </thead>
                            <tbody>" + string.Join(Environment.NewLine, products.Select(product => string.Format(/*lang=html*/ @"
                                <tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                </tr>", product.Id, product.Name, product.Price, product.IsAvailable))) + string.Format(/*lang=html*/ @"
                            </tbody>
                        </table>
                        <p>Total: {0}</p>
                    </body>  
                </html>", products.Sum(x => x.Price));
        }
    }
}