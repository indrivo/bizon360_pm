var usedSearch = false;

function GetTagList() {
    $.ajax({
        url: "/Tag/GetAllTags",
        type: "GET",
        traditional: true,
        success: function (result) {
            $(".tag-list").html(result);
            feather.replace();
        },
        error: function () {
            ErrorAlert("Error", "Could not load tag. Please try again later.");
            $(".tag-list").html(ERROR_MESSAGE);
            feather.replace();
        }
    });
    usedSearch = false;
    RenderTagForm();
}

function RenderTagForm() {
    $.get("/Tag/Create", function (result) {
        $(".modal-content.create-tag").append(result);
    });
}


function SearchRecruitingTags() {
    usedSearch = true;
    $.ajax({
        url: "/Tag/Search",
        type: "GET",
        data: { searchValue: $(".search-input").val() },
        success: function (result) {
            $(".tag-list").html(result);
            feather.replace();
        }
    });
}

$(document).ready(function () {

    $(".tag-list").html(LOADER);
    GetTagList();

    $.bindDelete({
        bindTo: "#btn-delete-tag",
        ajaxUrl: "/Tag/Delete",
        formId: "#form-delete-tag",
        modalId: "#modal-delete-tag",
        successMessage: "Tag has been deleted",
        errorMessage: "Could not remove tag",
        eventFunction: GetTagList
    });

    $(".search-button").click(function () {
        if ($(".search-input").val().length > 0)
            SearchRecruitingTags();
        else if (usedSearch === true)
            GetTagList();
    });

    $(".search-input").keypress(function (e) {
        if (e.keyCode === 13) {
            if ($(this).val().length > 0)
                SearchRecruitingTags();
            else
                if (usedSearch === true)
                    GetTagList();
        }
    });

});
