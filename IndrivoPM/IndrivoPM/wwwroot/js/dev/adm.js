// Generic scripts
// ---------------

function GetEntityList(actionUrl, htmlResult, tableCookieKey = "None") {
    $.ajax({
        url: actionUrl,
        type: "GET",
        data: { searchField: $(".search-input").val() },
        success: function (result) {
            $(htmlResult).html(result);
            if (tableCookieKey !== "None") {
                EnableDataTable(".bizon-datatable", tableCookieKey);
                $.bindColumnToggle({
                    cookieKey: tableCookieKey
                });
            }

            feather.replace();
        }
    });
}

function RefreshCount(parentId) {
    var tableElements = $("#table-" + parentId + " tr").length;
    var mapCount = $("#count-" + parentId);

    if (tableElements !== 0) {
        mapCount.html(tableElements - 1);
    } else {
        mapCount.html("0");
    }
}

var trHelper = function (e, ui) {
    ui.children().each(function () {
        $(this).width($(this).width());
    });
    return ui;
};

// Order table rows with drag and drop
// -----------------------------------
function OrderTableRows(options) {
    $(options.tBody).sortable({
        handle: "span.datatables-dragdrop",
        helper: trHelper,
        update: function (event, ui) {
            var itemIds = [];
            $(event.target).find(options.tRow).each(function () {

                var itemId = $(this).data("id");

                console.log(itemId);

                itemIds.push(itemId);
            });
            $.ajax({
                url: options.actionUrl,
                data: { itemIds: itemIds },
                type: "POST",
                success: function (result) { }
            });
        }
    });
}

// Returns parent ids
// -----------------
function GetCheckedParentIds() {
    var ids = [];

    $(".bizon-datatable").has(".datatables-check:checked").each(function () {
        ids.push($(this).attr("id").replace("table-", ""));
    });

    return ids;
}

// Get entities by parent
// ----------------------
function GetEntityByParent(actionUrl, parentId, tableCookieKey = "None") {
    $.ajax({
        url: actionUrl,
        type: "GET",
        data: {
            parentId: parentId
        },
        success: function (result) {

            $("#parent-" + parentId).html(result);

            if (tableCookieKey !== "None") {
                $.bindColumnToggle(tableCookieKey);
            }
            
        }
    });
}


// Application users container with scripts
// ----------------------------------------
function UserContainer() {
    this.getUsers = function() {
        GetEntityList("/ApplicationUsers/GetEmployeeList/", "#user-list");
    }
}

// Business units container with scripts
// -------------------------------------
function BusinessUnitContainer() {
    this.getBusinessUnits = function () {
        GetEntityList("/BusinessUnits/GetBusinessUnitList/", "#bu-list");
    }

    this.getDepartmentsByParent = function (ids = "None") {
        if (ids === "None") { 
            ids = GetCheckedParentIds();
            for (var i = 0; i < ids.length; i++) {
                GetEntityByParent("/Departments/GetDepartmentList/", ids[i]);
            }
        } else {
            GetEntityByParent("/Departments/GetDepartmentList/", ids);
        }
    }

}

// Job positions container with scripts
// ------------------------------------
function JobPositionContainer() {
    this.getJobList = function()
    {
        GetEntityList("/jobPositions/GetJobsList/", "#job-list");
    }
}

// Role and permissions container with scripts
// -------------------------------------------
function RolesContainer() {
    this.getRoleList = function() {
        GetEntityList("/Roles/GetRoleList/", "#role-list");
    }
}

// Department teams container with scripts
// ---------------------------------------
function TeamsContainer() {
    this.loader = function(pageId) {
        $(pageId).html(LOADER);
    }

    this.getTeamList = function() {
        GetEntityList("/DepartmentTeams/GetTeamList/", "#team-list");
    }

    this.getJobsByParent = function (ids = "None") {
        if (ids === "None") {
            ids = GetCheckedParentIds();
            for (var i = 0; i < ids.length; i++) {
                GetEntityByParent("/jobPositions/GetJobsList/", ids[i]);
            }
        } else {
            GetEntityByParent("/jobPositions/GetJobsList/", ids);
        }
    }
}

// Department container with scripts
// ---------------------------------
function DepartmentContainer() {
    this.loader = function(pageId) {
        $(pageId).html(LOADER);
    }

    this.getDepartments = function() {
        GetEntityList("/Departments/GetDepartmentList/", "#department-list");
    }

    this.getTeamsByParent = function (ids = "None") {

        if (ids === "None") {
            ids = GetCheckedParentIds();
            for (var i = 0; i < ids.length; i++) {
                GetEntityByParent("/DepartmentTeams/GetTeamList/", ids[i]);
            }
        } else {
            GetEntityByParent("/DepartmentTeams/GetTeamList/", ids);
        }
    }
}

// Sstp container with scripts
// --------------------------
function SstpContainer() {
    this.getSstp = function() {
        $("#product-list").html(LOADER);
        GetEntityList("/Sstp/GetProductList", "#parent-products");
        $("#service-list").html(LOADER);
        GetEntityList("/Sstp/GetServiceList", "#parent-services");
        $("#solution-list").html(LOADER);
        GetEntityList("/Sstp/GetSolutionList", "#parent-solutions");
        $("#technology-list").html(LOADER);
        GetEntityList("/Sstp/GetTechnologyList", "#parent-technologies");
    }

    this.getProducts = function() {
        GetEntityList("/Sstp/GetProductList", "#parent-products");
    }

    this.getServices = function () {
        GetEntityList("/Sstp/GetServiceList", "#parent-services");
    }

    this.getTechnologies = function(){
        $("#technology-list").html(LOADER);
        GetEntityList("/Sstp/GetTechnologyList", "#parent-technologies");
    }

    this.getSolutions = function () {
        GetEntityList("/Sstp/GetSolutionList", "#parent-solutions");
    }
}

// Activity type and trackers container with scripts
// -------------------------------------------------
function ServicesContainer() {
    this.getServices = function () {
        GetEntityList("/Services/GetActivityTypesList", "#activity-list");
    }

    this.getSubtypes = function (ids = "None") {
        if (ids === "None") {
            ids = GetCheckedParentIds();
            for (var i = 0; i < ids.length; i++) {
                GetEntityByParent("/Services/GetTrackersByActivity/", ids[i]);
            }
        } else {
            GetEntityByParent("/Services/GetTrackersByActivity/", ids);
        }
    }
}