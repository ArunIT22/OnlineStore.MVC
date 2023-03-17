/*
 AJAX Properties
   1) url, 2)type, 3)dataType, 4)contentType, 5) success, 6)error
 */

function getProductList() {
    $.ajax({
        url: 'https://localhost:7234/api/Products',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (result, status, xhr) {
            productList(result);
        },
        error: function (request, message, error) {
            errorHandler(request, message, error);
        }
    });
}

function productList(products) {
    $.each(products, function (index, product) {
        console.log(product);
        if ($("#tblProducts tbody").length == 0) {
            $("#tblProducts").append("<tbody></tbody>");
        }
        $("#tblProducts tbody").append(
            "<tr>" +
            "<td>"+product.product_Name+"</td>"+
            "<td>"+product.categoryName+"</td>"+
            "<td>"+product.listPrice+"</td>"+
            "<td>"+product.sellingPrice+"</td>"+
            "<td>" + product.discount + "</td>" +
            "</tr>"
        );
    });
}

function errorHandler(request, message, error) {
    var msg = "";
    msg += "Status Code :" + request.status + "\n";
    msg += "Message :" + request.statusText + "\n";
    console.log(msg);
}