$(document).ready(function () {

    var data = [
      [  "1234","56sde25 N 11th ave", "Phoenix", "AZ", "85013"],
      ["5678", "200 W Washington St,", "Phoenix", "AZ", "85003"]
  
    ];

    $('#properties-table').handsontable({
        data: data,
        colWidths: [200, 200,200,200,200,5],
        minSpareCols: 1,
        minSpareRows: 1,
        colHeaders: [ "Id","Address", "City", "State", "Zip Code",""],
        contextMenu: true
    });


    $('#validate-button').on('click', function () {

         tableData = $('#properties-table').handsontable('getData');

         

        var addresses = [];
        $.each(data, function (index, value) {
            var address = {};
            address.Id = value[0];
            address.StreetAddress = value[1];
            address.City = value[2];
            address.State = value[3];
            address.ZipCode = value[4];
            addresses.push(address);        
        });

        addresses.splice(addresses.length-1,  1);

  


        $('#tableData').val(JSON.stringify(addresses));

        console.log($('#table-data').val());
         submit();
    });

});

function submit()
{
    $('#addresses-form').submit();
}