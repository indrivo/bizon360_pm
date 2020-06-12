var usedSearch = false;

function GetColorTagList() {
    $.ajax({
        url: "/ColorTag/GetAllTags",
        type: "GET",
        traditional: true,
        success: function (result) {
            $(".colorTag-list").html(result);
            feather.replace();
        },
        error: function () {
            ErrorAlert("Error", "Could not load color tag. Please try again later.");
            $(".colorTag-list").html(ERROR_MESSAGE);
            feather.replace();
        }
    });
    usedSearch = false;
    RenderColorTagForm();
}

function RenderColorTagForm() {
    $.get("/ColorTag/Create", function (result) {
        $(".modal-content.create-colorTag").append(result);
    });
}


function SearchColorTags() {
    usedSearch = true;
    $.ajax({
        url: "/ColorTag/Search",
        type: "GET",
        data: { searchValue: $(".search-input").val() },
        success: function (result) {
            $(".colorTag-list").html(result);
            feather.replace();
        }
    });
}

$(document).ready(function () {

    $(".colorTag-list").html(LOADER);
    GetColorTagList();

    $.bindDelete({
        bindTo: "#btn-delete-colorTag",
        ajaxUrl: "/ColorTag/Delete",
        formId: "#form-delete-colorTag",
        modalId: "#modal-delete-colorTag",
        successMessage: "Color tag has been deleted",
        errorMessage: "Could not remove color tag",
        eventFunction: GetColorTagList
    });

    $(".search-button").click(function () {
        if ($(".search-input").val().length > 0)
            SearchColorTags();
        else if (usedSearch === true)
            GetColorTagList();
    });

    $(".search-input").keypress(function (e) {
        if (e.keyCode === 13) {
            if ($(this).val().length > 0)
                SearchColorTags();
            else
                if (usedSearch === true)
                    GetColorTagList();
        }
    });

});
