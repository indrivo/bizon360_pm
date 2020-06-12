var entityIds = [];

// Set params for send to controller
var params = {};

// On show modal set action name
var action;

// On show modal set controller name
var controller;

var entity = {
    parentIds: null
};

// Success message on modal
// -------------------------
function SuccessAlert(message) {
    Swal.fire({
        position: "top-end",
        type: "success",
        title: message,
        showConfirmButton: false,
        timer: 1500
    });
}

// Error message on modal
// -------------------------
function ErrorAlert(title, message) {
    Swal.fire({
        type: "error",
        title: title,
        text: message
    });
}

// On click create, show modal
// ---------------------------
function LoadModalCreate(options) {
    $(options.bindTo).click(function () {
        entity.parentIds = $(this).data("id");
        var id = $(this).data('id');
        var url = "/" + $(this).data('controller') + "/" + $(this).data('action');

        $("#modal-generic .modal-inner").html(LOADER);
        $("#modal-generic .modal-title").html(options.modalName);
        $("#modal-generic .modal-footer").addClass('d-none');
        $("#modal-generic .modal-inner").removeClass('p-3');

        console.log(url);
        $.ajax({
            url: url,
            type: "GET",
            data: { id: id },
            success: function (result) {
                $("#modal-generic .modal-inner").html(result);
                if (options.eventFunction != null) { options.eventFunction(result); }
            },
            error: function (result) {
                ErrorAlert("Could not load data");
            }
        });
    });
}

// On click update, show modal
// ---------------------------
function LoadModalUpdate(options) {
    $(options.bindTo).click(function () {

        var id = $(this).data('id');
        var url = "/" + $(this).data('controller') + "/" + $(this).data('action');

        $("#modal-generic .modal-inner").html(LOADER);
        $("#modal-generic .modal-title").html(options.modalName);
        $("#modal-generic .modal-footer").addClass('d-none');
        $("#modal-generic .modal-inner").removeClass('p-3');
        
        console.log(url + id);
        $.ajax({
            url: url,
            type: "GET",
            data: { id: id },
            success: function (result) {
                $("#modal-generic .modal-inner").html(result);
            },
            error: function (result) {
                ErrorAlert("Could not load data");
            }
        });
    });
}

// On click save
// ----------------------------
function SaveChanges(options) {
    $(options.bindTo).on("click", function (e) {
        var form = $(options.formId);
        var isValid = form.valid();
        if (!isValid) {
            e.preventDefault();
            return;
        }
        $(options.bindTo).prop('disabled', true);
        $.ajax({
            url: options.ajaxUrl,
            type: "POST",
            data: form.serialize(),
            success: function (result) {
                if (options.eventFunction != null)
                    options.eventFunction(result);
                form.trigger("reset");
                if (options.successMessage != null) {
                    Swal.fire({
                        position: "top-end",
                        type: "success",
                        title: options.successMessage,
                        showConfirmButton: false,
                        timer: 1500
                    });
                }
                if (options.new == null || !options.new)
                    $("#modal-generic").modal("hide");
            },
            error: function (result) {
                ErrorAlert(options.errorMessage);
            },
            complete: function() {
                $(options.bindTo).prop('disabled', false);

            }
        });
        e.preventDefault();
    });
}

// On click delete, show modal confirmation
// ----------------------------------------
function LoadModalDelete(options) {
    $(options.bindTo).click(function () {
        var id = $(this).data('id');
        var controller = $(this).data('controller');
        var action = $(this).data('action');
        var url = "/" + controller + "/" + action + "?id=" + id;

        $("#modal-generic .modal-footer").removeClass('d-none');
        $("#url-address").val(url);
        $("#entity-address").val(id);

        $("#modal-generic .modal-inner").addClass("p-3");
        $("#modal-generic .modal-inner").html("Delete \"" + $(this).data('name') + "\" " + options.type + " ?");
        $("#modal-generic .modal-title").html("Delete");
        $("#confirm-delete").html("Delete");
        $("#modal-generic").modal('show');
    });
}

// On click confirm delete item
// ----------------------------
function ConfirmDelete(options) {

    $("#confirm-delete").click(function () {
        $('#confirm-delete').prop('disabled', true);
        var url = $("#url-address").val();
        console.log(url);
        $.get(url)
            .done((result) => {

                if (options.eventFunction != null)
                    options.eventFunction(result);
                SuccessAlert(options.successMessage);
                $("#modal-generic").modal('hide');
                $("#modal-generic .modal-footer").addClass('d-none');


            })
            .fail((error) => {
                ErrorAlert(options.errorMessage);
                $("#modal-generic").modal('hide');
                $("#modal-generic .modal-footer").addClass('d-none');

            })
            .always(() => {
                $('#confirm-delete').prop('disabled', false);
            });
    });
}


// Modal for confirmation some action
// ----------------------------------------
function LoadModalConfirmAction(options) {
    $(options.bindTo).click(function () {

        var id = $(this).data('id');
        var controller = $(this).data('controller');
        var action = $(this).data('action');
        var url = "/" + controller + "/" + action + "?id=" + id;

        var btnSuccess = '<button type = "button" class= "btn btn-danger-custom font-weight-600" id = "' + options.confirmBtnId + '" >' + options.confirmBtnName + '</button >';
        $(options.modalId + " .modal-footer").removeClass('d-none');
        $(options.modalId +  " .modal-footer").html(btnSuccess );
        $("#url-address").val(url);

        $(options.modalId + " .modal-inner").addClass("p-3");
        $(options.modalId + " .modal-inner").html(options.messageConfirmation + " \"" + $(this).data('name') + "\" ?");
        $(options.modalId + " .modal-title").html(options.modalName);
        $(options.modalId).modal('show');
    });
}

// On click confirm action
// ----------------------------
function ModalConfirmAction(options) {
    $(options.confirmBtn).click(function () {
        $(options.confirmBtn).prop('disabled', true);
        var url = $("#url-address").val();
        console.log(url);
        $.get(url)
            .done((result) => {
                if (options.eventFunction != null)
                    options.eventFunction(result);
                SuccessAlert(options.successMessage);
                $(options.modalId).modal('hide');
                $(".modal-footer").addClass('d-none');


            })
            .fail((error) => {
                ErrorAlert(options.errorMessage);
                $("#modal-generic").modal('hide');
                $(".modal-footer").addClass('d-none');

            })
            .always(() => {
                $(options.confirmBtn).prop('disabled', false);
            });
    });
}


