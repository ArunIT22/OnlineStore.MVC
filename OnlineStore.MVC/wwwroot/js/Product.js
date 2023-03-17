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
            //productList(result);
            getProductListDataTable(result);
        },
        error: function (request, message, error) {
            errorHandler(request, message, error);
        }
    });
}

function productList(products) {
    console.log(products);
    $.each(products, function (index, product) {

        if ($("#tblProducts tbody").length == 0) {
            $("#tblProducts").append("<tbody></tbody>");
        }
        $("#tblProducts tbody").append(
            "<tr>" +
            "<td>" + product.product_Name + "</td>" +
            "<td>" + product.categoryName + "</td>" +
            "<td>" + product.listPrice + "</td>" +
            "<td>" + product.sellingPrice + "</td>" +
            "<td>" + product.discount + "</td>" +
            "</tr>"
        );
    });
}

function getProductListDataTable(products) {
    $("#tblProducts").dataTable({
        sorting: true,
        paging: true,
      
        "aaData": products,
        "columns": [
            { "data": "product_Name" },
            { "data": "categoryName" },
            { "data": "sellingPrice" },
            { "data": "listPrice" },
            { "data": "discount" },
        ]
    });
}

function errorHandler(request, message, error) {
    var msg = "";
    msg += "Status Code :" + request.status + "\n";
    msg += "Message :" + request.statusText + "\n";
    console.log(msg);
}

//Add New Product
function addNewProduct() {
    var product = {
        product_Name: $("#Product_Name").val(),
        categoryId: $("#CategoryId").val(),
        sellingPrice: $("#SellingPrice").val(),
        listPrice: $("#ListPrice").val(),
        discount: $("#Discount").val()
    };
    console.log(product);
    $.ajax({
        url: 'https://localhost:7234/api/Products',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(product),
        success: function (result) {
            alert(result.status);
            alert("Product created successfully, New Product Id :" + result["id"]);
        },
        error: function (request, message, error) {
            errorHandler(request, message, error);
        }
    });
}