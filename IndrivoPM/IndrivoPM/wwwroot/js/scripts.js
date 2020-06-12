var LOADER = "<div class=\"text-center py-2\"><div class=\"spinner-border\" role=\"status\"><span class=\"sr-only\">Loading...</span></div></div>";
var BUTTON_LOADER = "<span class=\"spinner-border spinner-border-sm\" role=\"status\" aria-hidden=\"true\"></span>";
var ERROR_MESSAGE = "<div class=\"text-center\"><i data-feather=\"frown\"></i></div><h5 class=\"text-center mt-2\">Something went wrong</h5>";

var activitiesView = {
    EmployeeView: 0,
    ListView: 1,
    SprintView: 2
}

function SuccessAlert(message) {
    Swal.fire({
        position: "top-end",
        type: "success",
        title: message,
        showConfirmButton: false,
        timer: 1500
    });
}

function ErrorAlert(title, message) {
    Swal.fire({
        type: "error",
        title: title,
        text: message
    });
}

function DisableButton(btnSelector) {
    $(btnSelector).prop("disabled", true).html(BUTTON_LOADER);
}

function EnableButton(btnSelector, text) {
    $(btnSelector).removeAttr("disabled").html(text);
}

function CheckPage() {
    return $("#current-page").val();
}

(function ($) {

    $.bindCreate = function (options) {
        $(options.bindTo).on("click", function (e) {
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
                    EnableButton(options.bindTo, options.buttonText != null ? options.buttonText : "Add");
                    form.trigger("reset");
                    SuccessAlert(options.successMessage);
                    if (options.new == null || !options.new)
                        $(options.modalId).modal("hide");
                },
                error: function (result) {
                    ErrorAlert(options.errorMessage);
                    EnableButton(options.bindTo, "Add");
                }
            });
            e.preventDefault();
        });
    }

    $.bindEdit = function (options) {
        $(options.bindTo).on("click", function (e) {
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
                    EnableButton(options.bindTo, options.buttonText == null ? "Update" : options.buttonText);
                    form.trigger("reset");
                    SuccessAlert(options.successMessage);
                    $(options.modalId).modal("hide");
                },
                error: function (result) {
                    ErrorAlert("Could not save changes");
                    EnableButton(options.bindTo, options.buttonText == null ? "Update" : options.buttonText);
                }
            });
            e.preventDefault();
        });
    }

    $.bindDelete = function (options) {
        $(options.bindTo).on("click", function (e) {
            DisableButton(options.bindTo);
            $.ajax({
                url: options.ajaxUrl,
                type: "POST",
                data: $(options.formId).serialize(),
                success: function (result) {
                    if (options.eventFunction != null)
                        options.eventFunction(result);
                    EnableButton(options.bindTo, options.buttonText == null ? "Delete" : options.buttonText);
                    SuccessAlert(options.successMessage);
                    $(options.modalId).modal("hide");
                },
                error: function (result) {
                    ErrorAlert(options.errorMessage);
                    EnableButton(options.bindTo, options.buttonText == null ? "Delete" : options.buttonText);
                }
            });
            e.preventDefault();
        });
    };

    $.loadEditForm = function(options) {
        $(options.bindTo).on("click", function () {
            $(options.modalBody).html(LOADER);
            $.ajax({
                url: options.ajaxUrl,
                type: "GET",
                data: options.data != null ? options.data : { id: $(this).attr("data-id") },
                success: function(result) {
                    $(options.modalBody).html(result);
                },
                error: function(result) {
                    ErrorAlert("Could not load data");
                }
            });
        });
    }

    $.deleteConfirmation = function(options) {
        $("body").on("click", options.bindTo, function() {
            $(options.modalBody).html("Delete \"" + $(this).attr("data-name") + "\" " + options.type + "?");
            $(options.inputId).val($(this).attr("data-id"));
            if (options.eventFunction != null) {
                options.eventFunction();
            }
        });
    }

    $.bindCollapseToggle = function(options) {
        $(".display-switcher").click(function() {
            if ($(this).hasClass("collapsed"))
                $(this).find(".display-icon.plus").addClass("d-none");
            else
                $(this).find(".display-icon.plus").removeClass("d-none");
        });

        $(".collapse-span").click(function() {
            var switcher = $(this).parent().find(".display-switcher");
            if (switcher.hasClass("collapsed"))
                switcher.find(".display-icon.plus").addClass("d-none");
            else
                switcher.find(".display-icon.plus").removeClass("d-none");
        });
    }

}(jQuery));

$(document).ready(function () {
    feather.replace();

    $("#sidebar-collapse").click(function () {
        $("#sidebar, #content, .bulk-actions-container").toggleClass("active");
    });

});