// Activate select2
// ----------------
function Select2() {
    $(document).ready(function() {
        $(".select2").select2({
            "width": "100%"
        });
    });
}

function OrderTableRows(options) {
    $(options.tBody).sortable({
        update: function (event, ui) {
            var itemIds = [];
            $(event.target).find(options.tRow).each(function () {

                var itemId = $(this).data("id");

                console.log(itemId);

                itemIds.push(itemId);
            });
            $.ajax({
                url: options.actionUrl,
                data: { itemIds: itemIds },
                type: "POST",
                success: function (result) { }
            });
        }
    });
}


$(document).ready(function () {
    $(".show-inactive").click(function () {

        var $this = $(this);

        if ($this.data("opened") === false) {
            $this.text("Hide inactive");
            $this.data("opened", true);

        } else {
            $this.text("Show inactive");
            $this.data("opened", false);
        }
    });
});

// Compare two Guids and return bool
// ---------------------------------
function GuidsAreEqual(guid1, guid2) {
    var isEqual = false;
    if (guid1 == null || guid2 == null) {
        isEqual = false;
    }
    else {
        isEqual = (guid1.replace(/[{}]/g, "").toLowerCase() == guid2.replace(/[{}]/g, "").toLowerCase());
    }
    return isEqual;
}

// Success message for modal
// --------------------------
function SuccessAlert(message) {
    Swal.fire({
        position: "top-end",
        type: "success",
        title: message,
        showConfirmButton: false,
        timer: 1500
    });
}

// Error message for modal
// -----------------------
function ErrorAlert(message) {
    Swal.fire({
        type: "error",
        title: "Error",
        text: message
    });
}

// Bind select list and multiple-select form another list
// ------------------------------------------------------
var selectedTeams = [];
var selectedJobs = [];

function BindSelectList(urlAction, dataParams, dataResult) {
    if (selectedTeams != null && selectedTeams.length === 0) { selectedTeams = $(dataResult.appendIn).val(); }
    if (selectedJobs != null && selectedJobs.length === 0) { selectedJobs = $("#job-list").val(); }

    $.ajax({
        url: urlAction,
        type: "POST",
        data: dataParams,
        traditional: true,
        success: function (result) {
            if (result == null || result.length === 0) {
                $(dataResult.appendIn)
                    .empty()
                    .html(`<option selected disabled>There are no entries.</option>`)
                    .attr("disabled", true)
                    .selectpicker("refresh");
                return;
            }

            var item = "";
            $(dataResult.appendIn).attr("disabled", false);

            if (dataResult.placeholder != undefined) {
                item = '<option selected disabled value="">' + dataResult.placeholder + "</option>";
            }

            $.each(result,
                function(i, team) {
                    item += '<option  value="' + team.value + '">' + team.text + "</option>";
                });

            $(dataResult.appendIn).html(item);

            if (selectedTeams != null && selectedTeams.length > 0) {
                for (var i = 0; i < selectedTeams.length; i++) {
                    $(dataResult.appendIn + ' option[value="' + selectedTeams[i] + '"]').attr("selected", true);
                }
            }

            if (selectedJobs != null) {
                    $('#job-list option[value="' + selectedJobs + '"]').attr("selected", true);
            }

            $(dataResult.appendIn).selectpicker("refresh");
        }
    });
}

// Get entities
// ------------
function GetEntities(urlAction, classResult) {
    $.ajax({
        url: urlAction,
        type: "GET",
        data: { id: classResult },
        success: function (result) {
            $(classResult).html(result);
            feather.replace();
        },
        error: function () {
            $(classResult).html("<div class=\"text-center\"><i data-feather=\"frown\"></i></div><h5 class=\"text-center mt-2\">Something went wrong</h5>");
            feather.replace();
        }
    });
}

// Get entities from another entity
// --------------------------------
function GetEntitiesFrom(options) {
    $.ajax({
        url: options.actionUrl,
        type: "GET",
        data: {
            id: options.entityId,
            searchField: options.searchStr
        },
        success: function (result) {
            $(options.divContentId).html(result);
        },
        error: function () {
            $(options.divContentId).html("<div class=\"text-center\"><i data-feather=\"frown\"></i></div><h5 class=\"text-center mt-2\">Something went wrong</h5>");
        }
    });
}


function GenericAjaxCall(options, dataParams) {
    $.ajax({
        url: options.ajaxUrl,
        type: options.ajaxType,
        data: dataParams,
        success: function (result) {
            if (options.eventFunction != null) { options.eventFunction(result)}
        },
        error: function () {
            //$(classResult).html("<div class=\"text-center\"><i data-feather=\"frown\"></i></div><h5 class=\"text-center mt-2\">Something went wrong</h5>");
            //feather.replace();
        }
    });
}

// data tables
// -----------
function DataTables (options) {
    $(options.tableId).dataTable({
        "dom": "tip",
        "pageLength": options.pageLength,
        "paging": options.paging,
        "orderMulti": false,
        "searching": true,
        "autoWidth": false,
        "columnDefs": [
            { "targets": "no-sort", "orderable": false }
        ],
        "language": {
            "info": "_START_-_END_ of _TOTAL_",
            "paginate": {
                "previous": `<img src="/svg/chevron-left.svg" />`,
                "next": `<img src="/svg/chevron-right.svg" />`
            }
        }
    });
};

// Search command
// Parameter 1 = '/Controller/Action'; 
// Parameter 2 = '<div class = ".jobs-list"> result... <div>'; 
var delaySearch;

function Search(urlAction, classAppendResult, searchDelay = true) {

    if (searchDelay) {
        clearTimeout(delaySearch);

        delaySearch = setTimeout(function() {
                $.ajax({
                    url: urlAction,
                    type: "GET",
                    data: { searchField: $(".search-input").val() },
                    success: function(result) {
                        $(classAppendResult).html(result);
                        feather.replace();
                    }
                });
            },
            800);
    } else {
        $.ajax({
            url: urlAction,
            type: "GET",
            data: { searchField: $(".search-input").val() },
            success: function(result) {
                $(classAppendResult).html(result);
                EnableDataTable(".bizon-datatable", "DepartmentsColumns");
                $.bindColumnToggle("DepartmentsColumns");
                feather.replace();
            }
        });
    }
}

// Remember sidebar if is collapsed
// --------------------------------
$(document).ready(function () {
    if (typeof (Storage) !== "undefined") {

        if (JSON.parse(localStorage.getItem("sidebarCollapse")) !== null) {

            var sidebarCollapse = JSON.parse(localStorage.getItem("sidebarCollapse"));

            $("#sidebar").attr("class", sidebarCollapse.sidebar);
            $("#content").attr("class", sidebarCollapse.content);
        }

        $("#sidebar-collapse").click(function () {
            setTimeout(function () {

                var sidebarCollapse = {
                    sidebar: $("#sidebar").attr("class"),
                    content: $("#content").attr("class")
                };

                localStorage.setItem("sidebarCollapse", JSON.stringify(sidebarCollapse));
            },
                500);
        });

    }
});