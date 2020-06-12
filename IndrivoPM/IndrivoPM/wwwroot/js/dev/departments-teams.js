$.bindCollapseToggle();

function Select2() {
    $(document).ready(function () {
        $(".select2").select2();
    });
}


// Get teams list on load
// --------------------------
function GetTeamsOnReady() {
    $(document).ready(function() {
    $("#show-active").html(LOADER);
    GetEntities("/DepartmentTeams/GetActiveTeams", "#show-active");
    $('#show-inactive').html(LOADER);
    GetEntities("/DepartmentTeams/GetInactiveTeams", '#show-inactive');

    });
}

// Get department list on load
// ----------------------------
function GetDepartmentsOnReady() {
    $(document).ready(function() {
        $(".active-departments").html(LOADER);
        GetEntities("/Departments/GetActiveDepartments", "#show-active");
        $(".inactive-departments").html(LOADER);
        GetEntities("/Departments/GetInactiveDepartments", "#show-inactive");
    });
}

// Search teams active and inactive
// --------------------------------
function SearchInTeams() {
    Search('/DepartmentTeams/GetActiveTeams/', '#show-active', false);
    Search('/DepartmentTeams/GetInactiveTeams/', '#show-inactive', false);
}

// Search departments active and inactive.
// --------------------------------------
function SearchInDepartments() {
    setTimeout(() => {Search('/Departments/GetActiveDepartments/', '#show-active', false);
    Search('/Departments/GetInactiveDepartments/', '#show-inactive', false);},800);

}


// Team details search for jobs
// ----------------------------
function SearchInJobs(teamId) {
    $.ajax({
        url: "/DepartmentTeams/DetailsGetJobList/",
        type: "GET",
        data: {
            id: teamId,
            searchField: $(".search-input").val()
        },
        success: function (result) {
            $(".job-list").html(result);
            feather.replace();
        },
        error: function (result) {
            Swal.fire({
                type: "error",
                title: "Error!",
                text: "Could not load data."
            });
        }
    });
}