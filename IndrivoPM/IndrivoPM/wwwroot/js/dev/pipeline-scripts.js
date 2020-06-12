var usedSearch = false;

function GetPipelineList() {
    $.ajax({
        url: "/RecruitingPipeline/GetAllRecruitingPipelines",
        type: "GET",
        traditional: true,
        success: function (result) {
            $(".pipeline-list").html(result);
            feather.replace();
        },
        error: function () {
            ErrorAlert("Error", "Could not load recruiting pipeline. Please try again later.");
            $(".pipeline-list").html(ERROR_MESSAGE);
            feather.replace();
        }
    });
    usedSearch = false;
    RenderPipelineForm();
}

function RenderPipelineForm() {
    $.get("/RecruitingPipeline/Create", function (result) {
        $(".modal-content.create-pipeline").html(result);
    });
}


function SearchRecruitingPipelines() {
    usedSearch = true;
    $.ajax({
        url: "/RecruitingPipeline/Search",
        type: "GET",
        data: { searchValue: $(".search-input").val() },
        success: function (result) {
            $(".pipeline-list").html(result);
            window.feather.replace();
        }
    });
}

$(document).ready(function () {

    $(".pipeline-list").html(LOADER);
    GetPipelineList();

    $.bindDelete({
        bindTo: "#btn-delete-pipeline",
        ajaxUrl: "/RecruitingPipeline/Delete",
        formId: "#form-delete-pipeline",
        modalId: "#modal-delete-pipeline",
        successMessage: "Pipeline has been deleted",
        errorMessage: "Could not remove pipeline",
        eventFunction: GetPipelineList
    });

    $(".search-button").click(function () {
        if ($(".search-input").val().length > 0)
            SearchRecruitingPipelines();
        else if (usedSearch === true)
            GetPipelineList();
    });

    $(".search-input").keypress(function (e) {
        if (e.keyCode === 13) {
            if ($(this).val().length > 0)
                SearchRecruitingPipelines();
            else
                if (usedSearch === true)
                    GetPipelineList();
        }
    });

});
