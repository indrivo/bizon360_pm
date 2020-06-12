var usedSearch = false;

function GetCandidateList(statuses, searchParam) {
    $.ajax({
        url: "/Candidate/GetAllCandidates",
        type: "GET",
        data: {
            statuses: statuses,
            searchParam: searchParam
        },
        traditional: true,
        success: function (result) {
            $(".candidate-list").html(result);
            feather.replace();
        },
        error: function () {
            ErrorAlert("Error", "Could not load candidates. Please try again later.");
            $(".candidate-list").html(ERROR_MESSAGE);
            feather.replace();
        }
    });
    usedSearch = false;
    RenderCandidateForm();
}

function RenderCandidateForm() {
    $.get("/Candidate/Create", function(result) {
        $(".modal-content.create-candidate").html(result);
    });
}

$(document).ready(function () {

    $(".candidate-list").html(LOADER);
    GetCandidateList(["New", "Go"], null);

    $(".generic_toggle").className = "current";

    $.bindDelete({
        bindTo: "#btn-delete-candidate",
        ajaxUrl: "/Candidate/Delete",
        formId: "#form-delete-candidate",
        modalId: "#modal-delete-candidate",
        successMessage: "Candidate has been deleted",
        errorMessage: "Could not remove candidate",
        eventFunction: GetCandidateList("New", null)
    });

    function search(listParam, searchParam) {
        if (searchParam.length > 0) {
            GetCandidateList(listParam, $(".search-input").val())
        }
        else if (usedSearch === true)
            GetCandidateList(listParam, null);
    }

    $(".search-button").click(function () {
        debugger;
        let _tabs = $("#CandidateTabs");
        let _class = _tabs.find(".active");
        let id= _class.attr("id");
        let searchParam = $(".search-input").val();
        switch (id) {
            case "_current":
                search(["New", "Go"], searchParam);
                $(".generic_toggle").className = "current";
                break;
            case "_nogo":
                search(["NoGo"], searchParam);
                $(".generic_toggle").className = "nogo";
                break;
            case "_lost":
                search(["Lost"], searchParam);
                $(".generic_toggle").className = "lost";
                break;
            case "_won":
                search(["Won"], searchParam);
                $(".generic_toggle").className = "won";
                break;
        }
    });

    $(".search-input").keypress(function (e) {
        if (e.keyCode === 13) {
            let _tabs = $("#CandidateTabs");
            let _class = _tabs.find(".active");
            let id = _class.attr("id");
            let searchParam = $(".search-input").val();
            switch (id) {
            case "_current":
                    search(["New", "Go"], searchParam);
                    $(".generic_toggle").className = "current";
                break;
            case "_nogo":
                search(["NoGo"], searchParam);
                $(".generic_toggle").className = "nogo";
                break;
            case "_lost":
                search(["Lost"], searchParam);
                $(".generic_toggle").className = "lost";
                break;
            case "_won":
                search(["Won"], searchParam);
                $(".generic_toggle").className = "won";
                break;
            }
        }
    });

});
    
$("#_current").click(function () {

    $("#tabs").tabs("option", "active", "#_current");

    GetCandidateList(["New", "Go"], null);
    $(".generic_toggle").attr('class', "current");
});

$("#_nogo").click(function () {

    $("#tabs").tabs("option", "active", "#_nogo");

    GetCandidateList(["NoGo"], null);
    $(".generic_toggle").attr('class', "nogo");
});

$("#_lost").click(function () {

    $("#tabs").tabs("option", "active", "#_lost");

    GetCandidateList(["Lost"], null);
    $(".generic_toggle").attr('class', "lost");
});

$("#_won").click(function () {

    $("#tabs").tabs("option", "active", "#_won");

    GetCandidateList(["Won"], null);
    $(".generic_toggle").attr('class', "won");
});