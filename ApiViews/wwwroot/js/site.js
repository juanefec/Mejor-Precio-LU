// Write your JavaScript code.

function searchBtn() {
    var texto = $("#searchInput").val();

    $.ajax({
        url: "http://localhost:5000/search/byName?nombre=" + texto,
        dataType: "json",
        type: "GET",
        success: function(response) {
            response.forEach(item => {
                if (texto == item.name) {
                    fullProduct(item.id);
                }
            });

            console.log(response);
        },
        error: function(err) {
            // do error checking
            alert("something went wrong");
            console.error(err);
        }
    });
}




function validatePassword() {
    var password = document.getElementById("password");
    var confirm_password = document.getElementById("ConfirmPassword");
    if (password.value != confirm_password.value) {
        confirm_password.setCustomValidity("Passwords Don't Match");
    } else {
        confirm_password.setCustomValidity('');
    }

    password.onchange = validatePassword;
    ConfirmPassword.onkeyup = validatePassword;
}


function fullProduct(id) {
    $.ajax({
        url: "http://localhost:5000/search/fullproduct/" + id,
        dataType: "json",
        type: "GET",
        success: function(response) {
            // success - for now just log it
            console.log(response);
            drop(response.storeList);
            createTable(response.storeList);
        },
        error: function(err) {
            // do error checking
            alert("something went wrong");
            console.error(err);
        }
    });
}

function createTable(storeList) {
    $("table").remove();
    var $table = "";
    $table = $('<table class="table" style="width:100%"/>');
    $table.append("<tr><th> Tienda </th><th> Dirección </th> <th> Precio </th></tr>");
    storeList.forEach(store => {
        $table.append('<tr><td>' + store.name + "</td><td>" + store.address + "</td><td>" + store.price.cost + '</td></tr>');
    });
    $('#productTable').append($table);
    $("#productTable").hide();
}

function HideMap() {
    $("#map").hide();
    $("#productTable").show();
}

function ShowMap() {
    $("#map").show();
    $("#productTable").hide();
}

var options = {
    url: function(phrase) {
        return "http://localhost:5000/search/byName?name=" + phrase;
    },

    getValue: "name"


};



$("#searchInput").easyAutocomplete(options);
$('div.easy-autocomplete').removeAttr('style');

$("#searchInput").keyup(function(event) {
    if (event.keyCode === 13) {
        $("#searchButton").click();
    }
});
$(document).ready(function() {
    var navItems = $('.admin-menu li > a');
    var navListItems = $('.admin-menu li');
    var allWells = $('.admin-content');
    var allWellsExceptFirst = $('.admin-content:not(:first)');
    allWellsExceptFirst.hide();
    navItems.click(function(e) {
        e.preventDefault();
        navListItems.removeClass('active');
        $(this).closest('li').addClass('active');
        allWells.hide();
        var target = $(this).attr('data-target-id');
        $('#' + target).show();
    });
});
$(function() {

    // We can attach the `fileselect` event to all file inputs on the page
    $(document).on('change', ':file', function() {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });

    // We can watch for our custom `fileselect` event like this
    $(document).ready(function() {
        $(':file').on('fileselect', function(event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' files selected' : label;

            if (input.length) {
                input.val(log);
            } else {
                if (log) alert(log);
            }

        });
    });

});

$(document).ready(function(){
  $("#searchForTable").on("keyup", function() {
    var value = $(this).val().toLowerCase();
    $("#tableBody tr").filter(function() {
      $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
  });
});