// ----------------------------
// Variables

var userList = $("#user-list");
var popperUsers = $("#popper-users");
var selectUser = $("#select-user");


// -----------------------
// Elements are hidden

popperUsers.hide();
selectUser.hide();


// --------------------------
// On user was choose

userList.change(function() {
    var userName = $("#user-list option:selected").text();
    var input = $("#message");

    input.val(input.val() + userName);
    popperUsers.hide();
    input.focus();
});

// ---------------------
// Hide popper on click cancel

function ClosePopper() {
    replayBox.hide();
}


// ---------------------------
// On click create, show modal

function ReplayGet(commentId, authorId) {
    $.ajax({
        url: "/MainComments/CreateSubComment",
        type: "GET",
        data: { id: commentId, authorId: authorId },
        traditional: true,
        success: function (result) {
            replayBox.html(result);
        }
    });
}

// -------------------------
// Update Comment/SubComment

function UpdateGet(id, actionName) {
    $.ajax({
        url: "/MainComments/" + actionName,
        type: "GET",
        data: { id: id},
        traditional: true,
        success: function (result) {
            replayBox.html(result);
        }
    });
}

// ---------------------
// SaveChanges
// On click save
// ----------------------------
function SendForm(options) {
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
                ClosePopper();
                GetComments();
            },
            complete: function () {
                ClosePopper();
                $(options.bindTo).prop('disabled', false);

            }
        });
        e.preventDefault();
    });
}


// ---------------------
// Input box onkeyup, send key code
function uniKeyCode(event) {
    var key = event.keyCode;

    if (key === 50) {
        selectUser.show();
        $("#select-user select").focus();

        var pInstance = new Popper(event.target,
            selectUser,
            {
                placement: "top"
            });
    }
    else {
        selectUser.hide();
    }
}

function OnUserSelected(inputId) {
$("#select-user select").change(function () {
    var userName = $("#select-user select option:selected").text();
    var input = $(inputId);
    input.val(input.val() + userName);
    $(this).hide();
    input.focus();
});
}

function AppendUserName(event, inputId) {
    var userName = event.target.find("option:selected").text();
    var input = $(inputId);
    input.val(input.val() + userName);
    $(event.target).hide();
    input.focus();
}




