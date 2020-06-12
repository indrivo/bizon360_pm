// Search in all cards
// ---------------------

var delayForSearch;

function SearchProjectService() {
    clearTimeout(delayForSearch);
    delayForSearch = setTimeout(() => {
            Search("/Sstp/GetProductList", "#product-list", false);
            Search("/Sstp/GetServiceList", "#service-list", false);
            Search("/Sstp/GetSolutionList", "#solution-list", false);
            Search("/Sstp/GetTechnologyList", "#technology-list", false);
        },
        800);
}


// Get list on load
// -----------------
function GetActivityServices() {
    GetEntities("/Services/GetActivityTypesList", ".activity-type-list"); 
}

function LoadActivityServices() {
    $(".activity-type-list").html(LOADER);
    GetEntities("/Services/GetActivityTypesList", ".activity-type-list"); 
}


function GetProjectServices () {
    $("#product-list").html(LOADER);
    GetEntities("/Sstp/GetProductList", "#product-list");
    $("#service-list").html(LOADER);
    GetEntities("/Sstp/GetServiceList", "#service-list");
    $("#solution-list").html(LOADER);
    GetEntities("/Sstp/GetSolutionList", "#solution-list");
    $("#technology-list").html(LOADER);
    GetEntities("/Sstp/GetTechnologyList", "#technology-list");
}

