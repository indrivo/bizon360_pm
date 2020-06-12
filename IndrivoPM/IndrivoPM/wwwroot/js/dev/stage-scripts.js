var usedSearch = false;

function GetStageList() {
    $.ajax({
        url: "/RecruitmentStage/GetAllRecruitmentStages",
        type: "GET",
        traditional: true,
        success: function (result) {
            $(".stage-list").html(result);
            feather.replace();
        },
        error: function () {
            ErrorAlert("Error", "Could not load recruitment stages. Please try again later.");
            $(".stage-list").html(ERROR_MESSAGE);
            feather.replace();
        }
    });
    usedSearch = false;
    RenderStageForm();
}

function RenderStageForm() {
    $.get("/RecruitmentStage/Create", function (result) {
        $(".modal-content.create-stage").append(result);
    });
}


function SearchRecruitmentStages() {
    usedSearch = true;
    $.ajax({
        url: "/RecruitmentStage/Search",
        type: "GET",
        data: { searchValue: $(".search-input").val() },
        success: function (result) {
            $(".stage-list").html(result);
            feather.replace();
        }
    });
}

$(document).ready(function () {

    $(".stage-list").html(LOADER);
    GetStageList();

    $.bindDelete({
        bindTo: "#btn-delete-stage",
        ajaxUrl: "/RecruitmentStage/Delete",
        formId: "#form-delete-stage",
        modalId: "#modal-delete-stage",
        successMessage: "Stage has been deleted",
        errorMessage: "Could not remove stage",
        eventFunction: GetStageList
    });

    $(".search-button").click(function () {
        if ($(".search-input").val().length > 0)
            SearchRecruitmentStages();
        else if (usedSearch === true)
            GetStageList();
    });

    $(".search-input").keypress(function (e) {
        if (e.keyCode === 13) {
            if ($(this).val().length > 0)
                SearchRecruitmentStages();
            else
                if (usedSearch === true)
                    GetStageList();
        }
    });

});
