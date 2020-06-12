var LOADER = '<img class="bizon-loader" src="/img/loader.gif" alt="Loading..." />';
var LOADER_SM = '<img class="bizon-loader loader-sm" src="/img/loader.gif" alt="Loading..." />';
var BUTTON_LOADER = "<span class=\"spinner-border spinner-border-sm\" role=\"status\" aria-hidden=\"true\"></span>";
var BOOTSTRAP_LOADER = '<div class="spinner-border" role="status"><span class="sr-only">Loading...</span></div>';

var deleteSuccessTitle = "";
var deleteSuccessMessage = "";
var deleteErrorTitle = "";
var deleteErrorMessage = "";
var deleteEventFunction = function () { }
var selectedRows = [];
var searchResult = [];
var searchIcons = [];

var redirectUrl = "";

var nameParam = "";
var dataParams = {};
var parentID = "";
var refreshTable = false;


var projectStatus = {
    Current: 0,
    OnHold: 1,
    Canceled: 2,
    Completed: 3
};


/**
 * Convert size in bytes to KB, MB, GB 
 */

function formatBytes(bytes, decimals) {
    if (bytes == 0) return '0 Bytes';
    var k = 1024,
        dm = decimals <= 0 ? 0 : decimals || 2,
        sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
        i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}


// ------------------------------
// Tabs
function openTab(evt, tabName) {
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(tabName).style.display = "block";
    evt.currentTarget.className += " active";
}

// --------------------------------------------
// Shows / hides container when button is clicked
function showInactive(item, containerId, table = false) {
    var THIS = $(item);
    if (THIS.attr("data-open") === "true") {
        $(containerId).attr("class", "d-none");
        THIS.attr("data-open", false);
        THIS.removeClass("active");
    } else {
        if (!table) {
            $(containerId).attr("class", "");
        }
        else {
            $(containerId).attr("class", "collapse-datatable");
        }
        THIS.attr("data-open", true);
        THIS.addClass("active");
    }
}

function GetCookie(key) {
    var start = document.cookie.indexOf(key + "=");
    if (start == -1) return null;

    var len = start + key.length + 1;

    if ((!start) && (key != document.cookie.substring(0, key.length))) {
        return null;
    }

    var end = document.cookie.indexOf(";", len);
    if (end == -1) end = document.cookie.length;

    return unescape(document.cookie.substring(len, end));
}

function EnableSummerNote(objectSelector, toolBar = null) {

    if (toolBar === null) {
        toolBar = [
            ['fontsize', ['fontsize']],
            ['fontname', ['fontname']],
            ['font', ['bold', 'italic', 'underline', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link']]
        ];
    }

    var editor = $(objectSelector);
    editor.summernote({
        dialogsInBody: true,
        dialogsFade: false,
        height: 100, // set editor height
        minHeight: 206, // set minimum height of editor
        maxHeight: 400, // set maximum height of editor
        focus: true, // set focus to editable area after initializing summernote
        popover: { //solve -> note-popover appearing in top left on page load
            image: [],
            link: [],
            air: []
        },
        toolbar: toolBar,
        callbacks: {
            onPaste: function (e) {
                var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');
                e.preventDefault();
                document.execCommand('insertText', false, bufferText);
            }
        }
    });

    editor.summernote("fontName", "Blinker");
    editor.summernote("fontSize", 14);
    editor.summernote('foreColor', 'black');


    //Tell the validator to ignore Summernote elements.
    $('form').each(function () {
        if ($(this).data('validator'))
            $(this).data('validator').settings.ignore = ".note-editor *";
    });
}

function EnableDataTable(tableClass, cookieKey, rowOrder = false, rowOrderIndex = 1) {
    $(document).on("click", "input[type='checkbox'].datatables-check", function () {
        if ($(this).prop("checked") === true) {
            $(this).parents("tr").addClass("bg-white");
        } else {
            if ($("[data-parent-id='" + $(this).attr("data-parent-id") + "'].datatables-check:checked").length < 1) {
                $("[id='bulk-" + $(this).attr("data-parent-id") + "'].datatables-bulk-check").prop("checked", false);
            }
            $(this).parents("tr").removeClass("bg-white");
        }

        if (CountCheckedRows() > 0) {
            $("#check-counter, .check-counter").html(CountCheckedRows());
            ShowBulkContainer();
        } else {
            HideBulkContainer();
        }
    });

    $("input[type='checkbox'].datatables-bulk-check").click(function () {
        var bulkId = $(this).attr("id");
        bulkId = bulkId.substring(bulkId.indexOf("-") + 1);
        if ($(this).prop("checked") === true) {
            $("[data-parent-id='" + bulkId + "'].datatables-check").each(function () {
                $(this).prop("checked", true);
                $(this).parents("tr").addClass("bg-white");
            });
        } else {
            $("[data-parent-id='" + bulkId + "']").each(function () {
                $(this).prop("checked", false);
                $(this).parents("tr").removeClass("bg-white");
            });
        }

        if (CountCheckedRows() > 0) {
            $("#check-counter, .check-counter").html(CountCheckedRows());
            ShowBulkContainer();
        } else {
            HideBulkContainer();
        }
    });

    var hiddenColumns = [];
    if (GetCookie(cookieKey) !== null) {
        hiddenColumns = GetCookie(cookieKey).split(",").map(Number);
        if (hiddenColumns[0] === 0) {
            hiddenColumns = [];
        } else {
            hiddenColumns.forEach(function (element) {
                $("[data-table-column-id='" + element + "'].datatables-column-toggle").prop("checked", false);
            });
        }
    }

    var dataTableOptions = {
        dom: "t",
        orderMulti: false,
        autoWidth: false,
        paging: false,
        columnDefs: [{ targets: "no-sort", orderable: false },
        { targets: hiddenColumns, visible: false }]
    }

    $(".datatables-th-dropdown .custom-control").click(function (e) {
        e.stopPropagation();
    });

    var dt = $(tableClass).DataTable(dataTableOptions);

    if (rowOrder) {
        dt.on('order.dt search.dt', function () {
            dt.column(rowOrderIndex, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                if (i < 9) {
                    cell.innerHTML = "0" + (i + 1);
                } else {
                    cell.innerHTML = i + 1;
                }
            });
        }).draw();
    }

    return dt;
}

function EnableDataTableFromLocalStorage(tableClass, cookieKey, rowOrder = false, rowOrderIndex = 1) {
    $(document).on("click", "input[type='checkbox'].datatables-check", function () {
        if ($(this).prop("checked") === true) {
            $(this).parents("tr").addClass("bg-white");
        } else {
            if ($("[data-parent-id='" + $(this).attr("data-parent-id") + "'].datatables-check:checked").length < 1) {
                $("[id='bulk-" + $(this).attr("data-parent-id") + "'].datatables-bulk-check").prop("checked", false);
            }
            $(this).parents("tr").removeClass("bg-white");
        }

        if (CountCheckedRows() > 0) {
            $("#check-counter, .check-counter").html(CountCheckedRows());
            ShowBulkContainer();
        } else {
            HideBulkContainer();
        }
    });

    $("input[type='checkbox'].datatables-bulk-check").click(function () {
        var bulkId = $(this).attr("id");
        bulkId = bulkId.substring(bulkId.indexOf("-") + 1);
        if ($(this).prop("checked") === true) {
            $("[data-parent-id='" + bulkId + "'].datatables-check").each(function () {
                $(this).prop("checked", true);
                $(this).parents("tr").addClass("bg-white");
            });
        } else {
            $("[data-parent-id='" + bulkId + "']").each(function () {
                $(this).prop("checked", false);
                $(this).parents("tr").removeClass("bg-white");
            });
        }

        if (CountCheckedRows() > 0) {
            $("#check-counter, .check-counter").html(CountCheckedRows());
            ShowBulkContainer();
        } else {
            HideBulkContainer();
        }
    });

    var hiddenColumns = [];
    if (window.localStorage.getItem('GlobalFilter') !== null) {
        hiddenColumns = window.localStorage.getItem('GlobalFilter').split(",").map(Number);
        if (hiddenColumns[0] === 0) {
            hiddenColumns = [];
        } else {
            hiddenColumns.forEach(function (element) {
                $("[data-table-column-id='" + element + "'].datatables-column-toggle").prop("checked", false);
            });
        }
    }

    var dataTableOptions = {
        dom: "t",
        orderMulti: false,
        autoWidth: false,
        paging: false,
        columnDefs: [{ targets: "no-sort", orderable: false },
        { targets: hiddenColumns, visible: false }]
    }

    $(".datatables-th-dropdown .custom-control").click(function (e) {
        e.stopPropagation();
    });

    var dt = $(tableClass).DataTable(dataTableOptions);
    for(var i = 0; i < dt.columns.length; i++)
    console.log(dt.columns[i]);

    if (rowOrder) {
        dt.on('order.dt search.dt', function () {
            dt.column(rowOrderIndex, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                if (i < 9) {
                    cell.innerHTML = "0" + (i + 1);
                } else {
                    cell.innerHTML = i + 1;
                }
            });
        }).draw();
    }

    return dt;
}

function CountCheckedRows() {
    return $(".datatables-check:checked").length;
}

function GetCheckedRows() {
    var checkedRows = [];

    $(".datatables-check:checked").each(function () {
        checkedRows.push($(this).attr("id"));
    });

    return checkedRows;
}

function ShowBulkContainer() {
    $(".bulk-actions-container").addClass("show");
    $(".bulk-hidden-space").removeClass("d-none");
}

function HideBulkContainer() {
    $(".bulk-actions-container").removeClass("show");
    $(".bulk-hidden-space").addClass("d-none");
}

function DeselectAll() {
    $(".datatables-check:checked").each(function () {
        $(this).prop("checked", false).parents("tr").removeClass("bg-white");
    });

    $(".datatables-bulk-check:checked").each(function () {
        $(this).prop("checked", false);
    });

    HideBulkContainer();
}

function DisableButton(button) {
    $(button).prop("disabled", true).html(BUTTON_LOADER);
}

function LoadMore($this, url, projectId) {
    DisableButton($this);
    $.ajax({
        url: url,
        type: "GET",
        data: {
            parentType: $this.attr("data-parent-type"),
            parentId: $this.attr("data-parent-id"),
            skip: $this.attr("data-skip"),
            filter: GetFilterValues(),
            projectId: projectId
        },
        traditional: true,
        success: function (result) {
            var tbody = $("#tbody-" + $this.attr("data-parent-id"));
            var dt = tbody.parents(".bizon-datatable").DataTable();
            $(result).filter("tr").each(function (i, v) {
                dt.row.add($(v));
            });
            tbody.append(result);

            var loadMoreButton = $("[data-parent-id='" + $this.attr("data-parent-id") + "'].load-more");

            if (tbody.children().length - loadMoreButton.attr("data-skip") < 10) {
                loadMoreButton.hide();
            } else {
                EnableButton($this, "Load more");
                loadMoreButton.attr("data-skip", tbody.children().length);
            }

            $("[data-parent-id='" + $this.attr("data-parent-id") + "'].entity-counter")
                .html(tbody.children().length);
        },
        error: function () {
            $.errorAlert({
                title: "Error",
                message: "Could not load activities."
            });
        }
    });
}


function EnableButton(button, text) {
    $(button).removeAttr("disabled").html(text);
}

function StopPropagation() {
    $(".collapse-button").click(function (e) {
        e.stopPropagation();
    });
    $(".collapse-container .dropdown-item").click(function (e) {
        e.stopPropagation();
        $(this).parents(".dropdown-menu.show").dropdown("hide");
    });

    $(".datatables-th-dropdown .custom-control").click(function (e) {
        e.stopPropagation();
    });
}

function StopCollapseButtonPropagation() {
    $(document).on("click", ".stop-propagation", function (e) {
        e.stopPropagation();
    });
}

function GetPriorityBadgeColor(priority) {
    switch (priority) {
        case "Critical":
            return "badge-outline-danger";
        case "High":
            return "badge-outline-warning";
        case "Medium":
            return "badge-outline-primary";
        default:
            return "badge-outline-info";
    }
}
function GetPriorityTextColor(priority) {
    switch (priority) {
        case "Critical":
            return "text-danger";
        case "High":
            return "text-warning";
        case "Medium":
            return "text-primary";
        default:
            return "text-info";
    }
}

function GetProgressBarColor(percentage) {
    return percentage <= 100 ? "bg-success" : "bg-danger";
}

function DeleteTableRowCollapseUpdate(entityId, noEntitiesMessage) {
    var row = $("tr[id='entity-" + entityId + "']");
    var parentCollapse = row.parents(".collapse-datatable");
    var parentTable = row.parents(".bizon-datatable").DataTable();

    var id = parentCollapse.attr("id");
    id = id.substring(id.indexOf("-") + 1);

    if (row.parents("tbody").children().length <= 1) {
        parentCollapse.html('<p class="mb-0 mt-2">' + noEntitiesMessage + '</p>');
    } else {
        row.remove();
        parentTable.row("#entity-" + entityId).remove();
    }

    var count = $("[id='count-" + id + "'].collapse-count");
    count.html(count.html() - 1);
}

function DeleteTableRow(entityId, collectionContainerId, noEntitiesMessage) {
    var row = $("tr[id='entity-" + entityId + "']");
    var parentTable = row.parents(".bizon-datatable").DataTable();

    if (row.parents("tbody").children().length <= 1) {
        $(collectionContainerId).html('<p class="mb-0 mt-2">' + noEntitiesMessage + '</p>');
    } else {
        row.remove();
        parentTable.row("#entity-" + entityId).remove();
    }
}

function DeleteRowCompleted(entityId, noEntitiesMessage, noOngoingEntitiesMessage) {
    var rowToDelete = $("tr[id='entity-" + entityId + "']");
    var parentCollapse = rowToDelete.parents(".collapse-datatable");
    var parentTable = rowToDelete.parents(".bizon-datatable").DataTable();
    var tBody = rowToDelete.parents("tbody");
    var parentId = tBody.parents(".table-wrapper").attr("data-parent-id");

    var hasAttribute = (typeof tBody.attr('id') !== typeof undefined && tBody.attr('id') !== false);

    if (hasAttribute && tBody.attr("id").startsWith("tbody-comp")) {
        if (tBody.children().length <= 1) {
            $(`[data-parent-id='${parentId}'].show-completed`).remove();
            tBody.parents(".completed-activities").remove();
        } else {
            rowToDelete.remove();
            parentTable.row("#entity-" + entityId).remove();
            var entityCounterCompleted = $(`[data-parent-id='${parentId}'].entity-counter-comp`);
            entityCounterCompleted.html(entityCounterCompleted.html() - 1);
        }

        if (!parentCollapse.find(".table-wrapper").length) {
            parentCollapse.find(".text-center").html(noEntitiesMessage).attr("class", "mb-0 mt-2 color-secondary");
            parentCollapse.find(".no-ongoing").remove();
        }
    } else {
        if (tBody.children().length <= 1) {
            tBody.parents(".table-wrapper").html(noOngoingEntitiesMessage)
                .attr("class", "mb-0 mt-2 color-secondary no-ongoing");
            parentCollapse.find(".entity-counter-container").remove();
        } else {
            rowToDelete.remove();
            parentTable.row("#entity-" + entityId).remove();
            var entityCounter = $("[data-parent-id='" + parentId + "'].entity-counter");
            entityCounter.html(entityCounter.html() - 1);
        }
    }

    var parentCount = $("[id='count-" + parentId + "'].collapse-count");
    parentCount.html(parentCount.html() - 1);
}

// Returns parent ids
function GetCheckedParentIds() {
    var ids = [];

    $(".bizon-datatable").has(".datatables-check:checked").each(function () {
        ids.push($(this).attr("id").replace("table-", ""));
    });

    return ids;
}

function Delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}

function CopyToClipboard(element) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).text()).select();
    document.execCommand("copy");
    $temp.remove();
    $.successAlert({
        message: "ID copied to clipboard."
    });
}

function Filter(selector, enumtype) {
    var $this = $(selector);

    var filterDOM =
        `<div class='dropdown datatables-th-dropdown'>` +
        `<button type='button' class='btn btn-primary' data-toggle='dropdown'>Filter</button>` +
        `<div class='dropdown-menu dropdown-menu-left filter-dropdown'></div>` +
        `</div>`;

    $this.html(filterDOM);

    console.log(enumtype);
}

// jQuery plugin to prevent double submission of forms
jQuery.fn.preventDoubleSubmission = function () {
    $(this).on('submit', function (e) {
        var $form = $(this);

        if ($form.data('submitted') === true) {
            // Previously submitted - don't submit again
            e.preventDefault();
        } else {
            // Mark it so that the next submit can be ignored
            $form.data('submitted', true);
        }
    });

    // Keep chainability
    return this;
};

// jQuery

(function ($) {

    $.bindFormModal = function (options) {
        $(document).on("click", options.bindTo, function (e) {
            e.stopPropagation();

            var id = $(this).data("id");
            var url = `/${$(this).data("controller")}/${$(this).data("action")}`;

            var data = {};

            if (options.bulk == null || !options.bulk) {
                if (options.data != null) {
                    var $this = $(this);
                    var dict = {};

                    $.each(options.data,
                        function (key, value) {
                            dict[key] = $this.attr(value);
                        });

                    data = dict;
                } else {
                    data = { id: id }
                }
            } else {
                var pName = $(this).data("param-name");

                if (pName === null || pName === undefined) {
                    if (options.optionalParam != null) {
                        data["entities"] = GetCheckedRows();
                        data[options.optionalParamName] = $(this).attr(options.optionalParam);
                    } else {
                        data = { entities: GetCheckedRows() }
                    }
                } else {
                    data[pName] = GetCheckedRows();
                }
            }

            if (options.modalSize != null) {
                $("#modal .modal-dialog")
                    .removeClass("modal-lg")
                    .addClass(options.modalSize);
            } else if (!$("#modal .modal-dialog").hasClass("modal-lg")) {
                $("#modal .modal-dialog").addClass("modal-lg");
            }

            if (!$("#modal .modal-delete-content").hasClass("d-none"))
                $("#modal .modal-delete-content").addClass("d-none");
            if (!$("#modal .modal-bulk-delete-content").hasClass("d-none"))
                $("#modal .modal-bulk-delete-content").addClass("d-none");

            $("#modal .modal-title").html(options.modalTitle);
            $("#modal .modal-inner").html(LOADER);
            $("#modal").modal("show");

            $.ajax({
                url: url,
                type: "GET",
                data: data,
                traditional: options.bulk == null || !options.bulk ? false : true,
                success: function (result) {
                    $("#modal .modal-inner").html(result);
                    if (options.eventFunction != null) options.eventFunction(result);
                },
                error: function () {
                    $.errorAlert({
                        title: "Error",
                        message: "Could not load data."
                    });
                }
            });
        });
    }

    $.bindDeleteModal = function (options) {
        $(document).on("click", options.bindTo, function (e) {
            e.stopPropagation();

            var url = "/" + $(this).data("controller") + "/" + $(this).data("action");

            nameParam = $(this).data("param-name");
            var parentId = $(this).data("parent-id");
            if (parentId != null) {
                if ($("#modal-delete-form #parent-id").length) {
                    $("#modal-delete-form #parent-id").val(parentId);
                } else {
                    $("#modal-delete-form").append("<input id='parent-id' name='parentId' type='hidden' value='" + parentId + "' />");
                }
            }

            if (options.colorButton != null) {
                $("#modal-bulk-delete-button").attr("class", "btn btn-success");
                $("#modal-delete-button").attr("class", "btn btn-success");
            } else {
                $("#modal-bulk-delete-button").attr("class", "btn btn-danger");
                $("#modal-delete-button").attr("class", "btn btn-danger");
            }

            if (options.modalSize != null) {
                $("#modal .modal-dialog")
                    .removeClass("modal-lg")
                    .addClass(options.modalSize);
            }

            if (options.bodyActionName == null) {
                options.bodyActionName = "Delete";
            }


            if (options.bulk == null || !options.bulk) {
                // Toggle bulk delete
                if (!$("#modal .modal-bulk-delete-content").hasClass("d-none")) {
                    $("#modal .modal-bulk-delete-content").addClass("d-none");
                }


                $("#modal #modal-delete-id").val($(this).data("id"));
                $("#modal .modal-delete-content").removeClass("d-none");

                if (options.timeLogDelete != null && options.timeLogDelete === true) {
                    $("#modal .modal-delete-content .modal-body")
                        .html("Please confirm time log deletion." + '<br /><b>' + $(this).attr("data-date") + "</b>, you logged <b>" + $(this).attr("data-time") + "</b> hour(s) on activity <b>" + $(this).attr("data-activity") + "</b>");
                } else {
                    $("#modal .modal-delete-content .modal-body")
                        .html(options.bodyActionName + ' "<b>' + $(this).attr("data-name") + '</b>" ' + options.type + "?");
                }
                deleteSuccessMessage =
                    options.successMessage != null ? options.successMessage : "Entity has been deleted.";
                deleteErrorMessage = options.errorMessage != null ? options.errorMessage : "Could not delete entity.";
                if (options.redirectUrl != null) {
                    redirectUrl = options.redirectUrl;
                } else {
                    redirectUrl = "";
                }
            } else {
                if ($(options.bindTo).data("get-parent") === true) {
                    parentID = GetCheckedParentIds();
                }
                // Toggle regular delete
                if (!$("#modal .modal-delete-content").hasClass("d-none")) {
                    $("#modal .modal-delete-content").addClass("d-none");
                }

                if (options.singleDelete) {
                    selectedRows.push($(this).data("id"));
                } else {
                    selectedRows = GetCheckedRows();
                }

                $("#modal .modal-bulk-delete-content").removeClass("d-none");
                $("#modal .modal-bulk-delete-content .modal-body").html(options.bodyActionName + " selected " + options.type + "?");
                deleteSuccessMessage = options.successMessage != null
                    ? options.successMessage
                    : "Selected entities have been deleted.";
                deleteErrorMessage = options.errorMessage != null
                    ? options.errorMessage
                    : "Could not delete selected entities.";
            }

            deleteSuccessTitle = options.successTitle != null ? options.successTitle : "Success";
            deleteErrorTitle = options.errorTitle != null ? options.errorTitle : "Error";
            deleteEventFunction = options.eventFunction != null ? options.eventFunction : function () { return true; }

            $("#modal").modal("show");
            $("#modal #modal-url").val(url);
            $("#modal .modal-title").html(options.modalTitle != null ? options.modalTitle : "Delete");
            $("#modal .modal-inner").html("");
            $("#modal-delete-button").text(options.bodyActionName);
            $("#modal-bulk-delete-button").text(options.bodyActionName);
        });
    }

    $.confirmDelete = function (options) {
        $("#modal-delete-button").click(function (e) {
            DisableButton("#modal-delete-button");
            console.log($("#modal-url").val());
            $.ajax({
                url: $("#modal-url").val(),
                type: "POST",
                data: $("#modal-delete-form").serialize(),
                success: function (result) {
                    deleteEventFunction(result);
                    $.successAlert({
                        title: deleteSuccessTitle,
                        message: deleteSuccessMessage
                    });
                    $("#modal").modal("hide");
                    if (redirectUrl.length > 0) {
                        setTimeout(function () { window.location = redirectUrl; }, 3000);
                    }
                },
                error: function () {
                    $.errorAlert({
                        title: deleteErrorTitle,
                        message: deleteErrorMessage
                    });
                },
                complete: function () {
                    EnableButton("#modal-delete-button", "Delete");
                }
            });

            e.preventDefault();
        });

        $("#modal-bulk-delete-button").click(function (e) {
            DisableButton("#modal-bulk-delete-button");

            // You can set your name of param
            var genericParamName = {}
            genericParamName["__RequestVerificationToken"] = $("#modal-bulk-delete-form [name=__RequestVerificationToken]").val();

            if (nameParam == null) {
                genericParamName["entities"] = selectedRows;
            } else {
                if (parentID !== null) {
                    genericParamName["parentId"] = parentID;
                }
                genericParamName[nameParam] = selectedRows;
                console.log(genericParamName);
            }

            $.ajax({
                url: $("#modal-url").val(),
                type: "POST",
                data: genericParamName,
                success: function (result) {
                    deleteEventFunction(result);
                    $.successAlert({
                        title: deleteSuccessTitle,
                        message: deleteSuccessMessage
                    });
                    $("#modal").modal("hide");
                },
                error: function () {
                    $.errorAlert({
                        title: deleteErrorTitle,
                        message: deleteErrorMessage
                    });
                },
                complete: function () {
                    EnableButton("#modal-bulk-delete-button", "Delete");
                }
            });
        });
    }

    $.saveChanges = function (options, resetForm = false) {
        $(options.bindTo).click(function (e) {
            var form = $(options.formId);
            var isValid = form.valid();

            if (!isValid) {
                e.preventDefault();
                return;
            }
            
            DisableButton(options.bindTo);
           
            $.ajax({
                url: options.ajaxUrl,
                type: "POST",
                data: form.serialize(),
                success: function (result) {
                    
                    if (options.eventFunction != null)
                        options.eventFunction(result);
                    if (options.hideSuccessAlert == null || !options.hideSuccessAlert) {
                        $.successAlert({
                            title: options.successTitle != null ? options.successTitle : "Success",
                            message: options.successMessage != null ? options.successMessage : "Your data has been saved."
                        });
                    }
                    if (resetForm) {
                        form.trigger("reset");
                        $(".filter-option-inner-inner").text("- Select -");
                        $("#summernote-create").summernote('reset');
                    }

                    if (options.new == null || !options.new)
                        $("#modal").modal("hide");
                },
                error: function () {
                    
                    $.errorAlert({
                        title: options.errorTitle != null ? options.errorTitle : "Error",
                        message: options.errorMessage != null ? options.errorMessage : "Could not save data."
                    });
                },
                complete: function () {
                    EnableButton(options.bindTo, options.buttonText != null ? options.buttonText : "Save");
                }
            });

            e.preventDefault();
        });
    }

    $.notificationAlert = function (options) {
        $.notify({
            title: options.title,
            message: options.message,
            url: options.url
        }, {
            type: "minimalist",
            url_target: "_blank",
            newest_on_top: true,
            animate: {
                enter: 'animated fadeInRight',
                exit: 'animated fadeOutRight'
            },
            mouse_over: "pause",
            template: '<div data-notify="container" class="col-xs-11 col-sm-3 col-11 alert alert-{0}" role="alert">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<span data-notify="title">{1}</span>' +
                '<span data-notify="message">{2}</span>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
        });
    }

    $.successAlert = function (options) {
        $.notify({
            title: options.title,
            message: options.message
        },
            {
                type: "success",
                delay: options.timer != null ? options.timer : 2500,
                animate: {
                    enter: 'animated fadeInRight',
                    exit: 'animated fadeOutRight'
                },
                newest_on_top: true,
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 col-11 alert alert-{0}" role="alert">' +
                    '<span data-notify="title">{1}</span>' +
                    '<span data-notify="message">{2}</span>' +
                    '</div>'
            });
    }

    $.errorAlert = function (options) {
        $.notify({
            title: options.title,
            message: options.message
        },
            {
                type: "danger",
                delay: options.timer != null ? options.timer : 0,
                animate: {
                    enter: 'animated fadeInRight',
                    exit: 'animated fadeOutRight'
                },
                newest_on_top: true,
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 col-11 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="title">{1}</span>' +
                    '<span data-notify="message">{2}</span>' +
                    '</div>'
            });
    }

    $.bindColumnToggle = function (options) {
        var hiddenColumns = [];

        $(".datatables-column-toggle").click(function () {
            var $this = $(this);
            var column = $this.attr("data-table-column-id");

            if (GetCookie(options.cookieKey) !== null) {
                if (GetCookie(options.cookieKey).split(",").map(Number)[0] !== 0) {
                    hiddenColumns = GetCookie(options.cookieKey).split(",");
                }
            }

            $(".bizon-datatable").each(function () {
                var data = $(this).dataTable().toArray();

                if ($this.prop("checked") === false) {
                    data.forEach(function (element) {
                        $(element).DataTable().columns().column(column).visible(false);
                    });

                    if (!hiddenColumns.includes(column)) {
                        hiddenColumns.push(column);
                    }
                } else {
                    data.forEach(function (element) {
                        $(element).DataTable().columns().column(column).visible(true);
                    });

                    if (hiddenColumns.includes(column)) {
                        hiddenColumns.splice(hiddenColumns.indexOf(column), 1);
                    }
                }
            });

            document.cookie = options.cookieKey + "=" + hiddenColumns;
        });
    }

    $.bindColumnToggleLocalStorage = function (options) {
        $(".datatables-column-toggle").click(function () {

            var $this = $(this);
            var column = $this.attr("data-table-column-id");

            var hiddenColumns = [];
            if (window.localStorage.getItem('GlobalFilter') !== null) {
                if (window.localStorage.getItem('GlobalFilter').split(",").map(Number)[0] !== 0) {
                    hiddenColumns = window.localStorage.getItem('GlobalFilter').split(",");
                }
            }

            $(".dt-global").each(function () {
                debugger;
                var data = $(this).dataTable().toArray();

                if ($this.prop("checked") === false) {
                    data.forEach(function (element) {
                        $(element).DataTable().columns().column(column).visible(false);
                    });

                    if (!hiddenColumns.includes(column)) {
                        hiddenColumns.push(column);
                    }
                } else {
                    data.forEach(function (element) {
                        $(element).DataTable().columns().column(column).visible(true);
                    });

                    if (hiddenColumns.includes(column)) {
                        hiddenColumns.splice(hiddenColumns.indexOf(column), 1);
                    }
                }
            });

            window.localStorage.setItem('GlobalFilter', hiddenColumns);
        });
    }

    $.bindSearch = function (options) {
        var hiddenContents = [];

        $(".search-input").keyup(function () {
            var searchValue = $(this).val();
            if (searchValue.length > 0) {
                $(".parent-content").each(function () {
                    var parentContent = $(this);
                    if (parentContent.find(".bizon-datatable").length > 0) {

                    } else {
                        // Hide project groups without any tables
                        if (!parentContent.hasClass("d-none")) {
                            parentContent.addClass("d-none");
                            hiddenContents.push(parentContent);
                        }
                    }
                });
            } else {
                $.each(hiddenContents, function (key, value) {
                    if (value.hasClass("d-none")) {
                        value.removeClass("d-none");
                    }
                });

                hiddenContents = [];
            }

            $(".bizon-datatable:not(.ignore-search)").each(function () {
                $(this).DataTable().search(searchValue).draw();

                var tableParent = $(this).parents(".collapse-datatable");
                var groupId = tableParent.attr("id");
                groupId = groupId.substring(groupId.indexOf("-") + 1);

                if ($(this).find(".dataTables_empty").length != 0) {
                    var parentContent = $(`[id='${groupId}'].parent-content`);
                    if (!parentContent.hasClass("d-none")) {
                        $(`[id='${groupId}']`).addClass("d-none");
                    }
                } else {
                    if (searchValue.length > 0) {
                        var collapse = $("[id='parent-" + groupId + "']");
                        if (!collapse.hasClass("show")) {
                            collapse.addClass("show");
                            searchResult.push(collapse);

                            var card = $("[href='#parent-" + groupId + "'].collapse-container");
                            card.attr("aria-expanded", true);
                            searchIcons.push(card);
                        }
                    } else {
                        $.each(searchResult, function (key, value) {
                            if (value.hasClass("show")) {
                                value.removeClass("show");
                            }
                        });
                        $.each(searchIcons, function (key, value) {
                            value.attr("aria-expanded", false);
                        });
                        searchResult, searchIcons = [];
                    }

                    var content = $(`[id='${groupId}'].parent-content`);
                    if (content.hasClass("d-none")) {
                        $(`[id='${groupId}']`).removeClass("d-none");
                    }
                }

                var noResults = $(".no-results");
                if (searchValue.length > 0 && $(".parent-content:not(.d-none)").length < 1) {
                    if (noResults.hasClass("d-none")) {
                        noResults.removeClass("d-none");
                    }
                } else {
                    if (!noResults.hasClass("d-none")) {
                        noResults.addClass("d-none");
                    }
                }
            });
        });
    }

    $.bindFilter = function (options) {
        // Dropdown container
        var container = $(options.container);

        var filterDom =
            '<div class="dropdown datatables-th-dropdown">' +
            '<button type="button" class="btn btn-outline-primary btn-filter p-0" data-toggle="dropdown"><span class="filter-icon"></span></button>' +
            '<div class="dropdown-menu dropdown-menu-left filter-dropdown"></div>' +
            '</div>';

        var filterFlexWrapper = `<div class="filter-wrapper ml-1"></div>`;

        container.html(filterDom);
        container.append(filterFlexWrapper);

        var dropdown = container.find(".dropdown-menu");
        var wrapper = container.find(".filter-wrapper");
      
        // Generate dropdown items
        $(options.types).each(function (key, value) {
            var type =
                `<div class="custom-control custom-checkbox"><input type="checkbox" class="custom-control-input filter-toggle" id="filter-${value.name}" data-value="${value.value}" ${value.default === true ? "checked" : ""
                } /><label class="custom-control-label" for="filter-${value.name}">${value.name}<span>${value.description}</span></label></div>`;

            dropdown.append(type);
        });

        $(".datatables-th-dropdown .custom-control").click(function (e) {
            e.stopPropagation();
        });

        var activeFiltersCount = 0;

        // Read cookie values onload
        var activeFilters = [];
        if (GetCookie(options.cookieKey) !== null) {
            if (GetCookie(options.cookieKey).split(",").map(Number)[0] !== 0)
                activeFilters = GetCookie(options.cookieKey).split(",").map(Number);

            $("input[type='checkbox'].filter-toggle").each(function () {
                $(this).prop("checked", false);
            });

            if (!$.isEmptyObject(activeFilters)) {
                $(activeFilters).each(function (key, value) {
                    $("input[data-value='" + value + "'].filter-toggle").prop("checked", true);
                   
                    activeFiltersCount++;
                });
            } else {
                $(options.types).each(function (key, value) {
                    if (value.default === true) {
                        activeFilters.push(value.value);
                        $("input[data-value='" + value.value + "'].filter-toggle").prop("checked", true);
                        activeFiltersCount++;
                    }
                });

                document.cookie = options.cookieKey + "=" + activeFilters;
            }
        } else {
            $(options.types).each(function (key, value) {
                if (value.default === true) {
                    activeFilters.push(value.value);
                    activeFiltersCount++;
                }
            });

            document.cookie = options.cookieKey + "=" + activeFilters;
        }

        CheckFilters();

        // Bind checkbox click event
        $(".filter-toggle").click(function () {
            var toggle = $(this);
            var enumValue = toggle.data("value").toString();
            
            if (GetCookie(options.cookieKey) !== null) {
                if (GetCookie(options.cookieKey).split(",").map(Number)[0] !== 0) {
                    activeFilters = GetCookie(options.cookieKey).split(",");
                }
            }

            if (toggle.prop("checked") === true) {
                if (!activeFilters.includes(enumValue)) {
                    activeFilters.push(enumValue);
                }
                $("[data-value='" + enumValue + "'].active-filter").removeClass("d-none");
            } else {
                if (activeFilters.includes(enumValue)) {
                    activeFilters.splice(activeFilters.indexOf(enumValue), 1);
                }
                $("[data-value='" + enumValue + "'].active-filter").addClass("d-none");
            }

            $("#filter-counter").html(activeFilters.length);
            if (activeFilters.length > 0) $(".active-filter").removeClass("d-none");

            document.cookie = options.cookieKey + "=" + activeFilters;
            CheckFilters();

            if (options.refreshFunction != null)
                options.refreshFunction();
        });

        var badge = `<div class="badge badge-outline-primary active-filter"><span id='filter-counter'>${activeFiltersCount}</span> filters<span class="cancel-filter"></span></div>`;

        wrapper.append(badge);

        // Bind badge close event
        $(".cancel-filter").click(function () {

            $(this).parent().addClass("d-none");

            $(".filter-toggle").each(function() { $(this).prop("checked", false) });
            activeFilters = [];

            document.cookie = options.cookieKey + "=" + activeFilters;
            CheckFilters();

            if (options.refreshFunction != null)
                options.refreshFunction();
        });
    }

}(jQuery));

// ------

function CheckFilters() {
    var activeFilters = $(".filter-toggle:checked").length;

    if (activeFilters >= 1) {
        $(".btn-filter").removeClass("btn-outline-primary").addClass("btn-primary");
    } else {
        $(".btn-filter").removeClass("btn-primary").addClass("btn-outline-primary");
    }
}

function GetFilterValues() {
    var result = [];

    $(".filter-toggle:checked").each(function() {
        result.push($(this).attr("data-value"));
    });

    return result;
}

function FilterRows() {
    var filters = [];
    $(".filter-toggle:checked").each(function () {
        filters.push($(this).attr("data-value"));
    });

    $(".entity-tr").each(function () {
        if (!filters.includes($(this).attr("data-status"))) {
            if (!$(this).hasClass("d-none")) {
                $(this).addClass("d-none");
            }
        } else {
            if ($(this).hasClass("d-none")) {
                $(this).removeClass("d-none");
            }
        }
    });

    $(".parent-content").each(function() {
        var count = $(this).find(".entity-tr:not(.d-none)").length;
        $(this).find(".collapse-count").html(count);
    });
}

$(document).ready(function () {

    feather.replace();

    var $hamburger = $(".hamburger");
    var $slideMenu = $(".main-nav-slide");
    var $mainContent = $(".main-content");
    var $navUserButton = $(".nav-user-button");
    var $sidebarClose = $(".side-nav-close");
    var $sidebar = $(".side-nav");
    var $overlay = $(".overlay");

    $hamburger.on("click", function (e) {
        $hamburger.toggleClass("is-active");
        $slideMenu.toggleClass("is-active");
        $mainContent.toggleClass("is-active");
    });

    $navUserButton.on("click", function (e) {
        $sidebar.addClass("is-active");
        $overlay.addClass("is-active");
    });

    $sidebarClose.on("click", function (e) {
        $sidebar.removeClass("is-active");
        $overlay.removeClass("is-active");
    });

    $overlay.on("click", function (e) {
        if ($(this).hasClass("is-active")) {
            $sidebar.removeClass("is-active");
            $overlay.removeClass("is-active");
        }
    });

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('#top-scroll').addClass("is-active");
        } else {
            $('#top-scroll').removeClass("is-active");
        }
    });
    $('#top-scroll').click(function () {
        $("html, body").animate({ scrollTop: 0 }, 500);
        return false;
    });

    $("#deselect-all").click(function () {
        DeselectAll();
    });

    $("#global-search").keyup(Delay(function () {
        var main = $("#main-content");
        var searchResultContainer = $("#global-search-result");

        if ($(this).val().length < 1) {
            if (main.hasClass("d-none")) {
                main.removeClass("d-none");
            }

            if (!searchResultContainer.hasClass("d-none")) {
                searchResultContainer.addClass("d-none");
                searchResultContainer.html("");
            }

            return;
        }

        $.ajax({
            url: "/Activities/SearchAllActivities",
            type: "GET",
            data: { searchValue: $(this).val() },
            success: function (result) {
                if (!main.hasClass("d-none")) {
                    main.addClass("d-none");
                }

                if (searchResultContainer.hasClass("d-none")) {
                    searchResultContainer.removeClass("d-none");
                }
                searchResultContainer.html(result);
            },
            error: function () {
                $.errorAlert({
                    title: "Error",
                    message: "Search failed."
                });
            }
        });
    }, 500));

    $("body").on("click", "#global-reset", function () {
        var searchInput = $("#global-search");
        var searchResultContainer = $("#global-search-result");
        var main = $("#main-content");

        if (main.hasClass("d-none")) {
            main.removeClass("d-none");
        }

        if (!searchResultContainer.hasClass("d-none")) {
            searchResultContainer.addClass("d-none");
            searchResultContainer.html("");
        }

        searchInput.val("");
    });

    $(".dark-mode-switcher").click(function () {
        var body = $("body");
        var layerClass = ".top-layer";
        var layers = document.querySelectorAll(layerClass);
        for (const layer of layers) {
            layer.classList.toggle("active");
        }

        setTimeout(function () {
            body.toggleClass("dark-theme");

            if (body.hasClass("dark-theme")) {
                document.cookie = "DarkTheme=true;path=/";
            } else {
                document.cookie = "DarkTheme=false;path=/";
            }
        }, 500);
    });

});