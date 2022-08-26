// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
   
    
    /* Source: https://stackoverflow.com/questions/20128472/bootstrap-3-0-full-length-body-sidebar */
    var document_height = $(document).height();
    var sidebar = $('.sidebar');
    var sidebar_height = sidebar.height();

    if (document_height > sidebar_height) {
        sidebar.css('height', document_height - 50);
    }

    /* Personal research. Collapse the side bars onload*/
    $(document).ready(function () {
        var left = $("#sidebar-left");
        var right = $("#sidebar-right");
        var content = $("#content");
        var openSidebarsCount = 0;
        var contentClass = "";

        //left.toggleClass("collapsed");
        right.toggleClass("collapsed");
        contentClass = "col-md-10";
        // apply class to content
        content.removeClass("col-md-12 col-md-10 col-md-8")
            .addClass(contentClass);
    });

/* Source:  https://www.broculos.net/2015/08/how-to-build-collapsible-sidebars-with.html
    https://github.com/nunof07/bootstrap-collapsible-sidebar
*/

    function toggleSidebar(side) {
        if (side !== "left" && side !== "right") {
            return false;
        }
        var left = $("#sidebar-left"),
            right = $("#sidebar-right"),
            content = $("#content"),
            openSidebarsCount = 0,
            contentClass = "";

        // toggle sidebar
        if (side === "left") {
            left.toggleClass("collapsed");
        } else if (side === "right") {
            right.toggleClass("collapsed");
        }

        // determine number of open sidebars
        if (!left.hasClass("collapsed")) {
            openSidebarsCount += 1;
        }

        if (!right.hasClass("collapsed")) {
            openSidebarsCount += 1;
        }

        // determine appropriate content class
        if (openSidebarsCount === 0) {
            contentClass = "col-md-12";
        } else if (openSidebarsCount === 1) {
            contentClass = "col-md-10";
        } else {
            contentClass = "col-md-8";
        }

        // apply class to content
        content.removeClass("col-md-12 col-md-10 col-md-8")
            .addClass(contentClass);
    }

    $(".toggle-sidebar-left").click(function () {
        toggleSidebar("left");

        return false;
    });
    $(".toggle-sidebar-right").click(function () {
        toggleSidebar("right");

        return false;
    });
});

$(document).ready(function () {

    var jobCount = $('#list .in').length;
    $('.list-count').text(jobCount + ' items');

    $("#search-text").keyup(function () {

        var searchTerm = $("#search-text").val();
        var listItem = $('#list').children('li');

        var searchSplit = searchTerm.replace(/ /g, "'):containsi('");

        $.extend($.expr[':'], {
            'containsi': function (elem, i, match, array) {

                return (elem.textContent || elem.innerText || '').toLowerCase()
                    .indexOf((match[3] || "").toLowerCase()) >= 0;
            }
        });

        $("#list li").not(":containsi('" + searchSplit + "')").each(function (e) {
            $(this).addClass('hiding out').removeClass('in');
            setTimeout(function () {
                $('.out').addClass('hidden');
            }, 300);
        });

        $("#list li:containsi('" + searchSplit + "')").each(function (e) {
            $(this).removeClass('hidden out').addClass('in');
            setTimeout(function () {
                $('.in').removeClass('hiding');
            }, 1);
        });

        var jobCount = $('#list .in').length;
        $('.list-count').text(jobCount + ' items');

        if (jobCount === '0') {
            $('#list').addClass('empty');
        }
        else {
            $('#list').removeClass('empty');
        }

    });

    function searchList() {

        var listArray = [];

        $("#list li").each(function () {
            var listText = $(this).text().trim();
            listArray.push(listText);
        });

        //$('#search-text').autocomplete({
        //    source: listArray
        //});

    }

    searchList();

});

$(document).ready(function () {

    //$(function () {
    //    $('button[data-toggle="modal"]').click(function (event) {
    //        var url = $(this).data('url');
    //        $.get(url).done(function (data) {
    //            $('#modal-placeholder').html(data);
    //            $('#modal-placeholder > .modal').modal('show');
    //        });
    //    });
    //});

    $(function () {
        var placeholderElement = $('#modal-placeholder');
        $('button[data-toggle="modal"]').click(function (event) {
            var url = $(this).data('url');
            $.get(url).done(function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            });
        });
    });


});