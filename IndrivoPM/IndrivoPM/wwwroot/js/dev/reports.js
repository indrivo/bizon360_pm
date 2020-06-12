var btnLoader = ' <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
$(document).ready(function () {
    $(".selectpicker").selectpicker();

    $(".datepicker").datepicker({
        format: "yyyy/mm/dd",
        maxViewMode: 3,
        todayBtn: "linked",
        autoclose: true,
        orientation: "bottom auto"
    });

    $("#emp-start-date, #emp-due-date").change(function () {
        startDate = $("#emp-start-date");
        dueDate = $("#emp-due-date");

        // Validation date time.
        validationRangeDate(startDate, dueDate);
    });

    $("#start-date, #due-date").change(function () {
        startDate = $("#start-date");
        dueDate = $("#due-date");

        // Validation date time.
        validationRangeDate(startDate, dueDate);
    });
});

// Validation range date.
// Pass jquery objects.
function validationRangeDate(startDate, dueDate) {
    // Check if start and end date are empty.
    if (startDate.val() === "") {
        startDate.addClass("is-invalid");
        startDate.closest("div").find("span").removeClass("d-none");

        if (dueDate.val() === "") {
            dueDate.addClass("is-invalid");
            dueDate.closest("div").find("span").removeClass("d-none");
        }
        return false;
    }

    // Check range between start and end date.
    if (!CheckDateRange(startDate, dueDate)) return false;

    // Hide invalid messages.
    startDate.removeClass("is-invalid");
    startDate.closest("div").find("span").addClass("d-none");
    dueDate.removeClass("is-invalid");
    dueDate.closest("div").find("span").addClass("d-none");
    return true;
}

// Validation dropdown and show msg,
// returns true if validation is ok.
function validationDropdown(elementId) {
    if (elementId.val().length === 0) {
        elementId.addClass("is-invalid");
        elementId.parent().parent().find("span.validation").removeClass("d-none");
        return false;
    }

    // Hide invalid messages.
    elementId.removeClass("is-invalid");
    elementId.parent().parent().find("span.validation").addClass("d-none");
    return true;
}

// ---------------------------------------
// Check range between start and end date, 
// returns true if date range is ok.
function CheckDateRange(startDate, dueDate) {

    // Check if start date is bigger than due date.
    if (startDate.val() > dueDate.val()) {
        startDate.addClass("is-invalid");
        startDate.closest("div").find("span").find("span").text("Start date cannot be smaller than the due date.");
        startDate.closest("div").find("span").removeClass("d-none");

        // Check if due date is smaller than start date.
        if (dueDate.val() < startDate.val()) {
            dueDate.addClass("is-invalid");
            dueDate.closest("div").find("span").find("span").text("Due date cannot be smaller than the start date.");
            dueDate.closest("div").find("span").removeClass("d-none");
        }
        return false;
    }
    return true;
}

// Disable button.
function DisableBtn(button) {
    button.attr("disabled", "true");
}

// Enable button.
function EnableBtn(button) {
    button.removeAttr("disabled");
}

// Generate report and display on ui.
function generateProjectReport(button, actionURL) {
    // Variables
    projectIds = $("#select-project");
    startDate = $("#start-date");
    dueDate = $("#due-date");
    divResult = $("#report-project");

    divResult.html(LOADER);
    // Validation inputs.
    if (!validationRangeDate(startDate, dueDate)) return;
    if (!validationDropdown(projectIds)) return;

    button.setAttribute("disabled", "true");

    $.ajax({
        type: "POST",
        url: actionURL,
        traditional: true,
        data: {
            projectIds: projectIds.val(),
            startDate: startDate.val(),
            dueDate: dueDate.val()
        },
        success: function (result) {
            divResult.html(result);
        },
        error: function () {
            divResult.html("Could not load data.");

        },
        complete: function () {
            button.removeAttribute("disabled");
        }
    });
}

// Generate teams filtered report and display on ui.
function generateTeamsFilteredReport(button, actionURL) {
    projectIds = $("#select-project-for-teams").val();
    employeeIds = $("#select-employee-for-teams").val();
    activityTypeIds = $("#select-activity-types").val();
    activityStatuses = $("#select-activity-statuses").val();
    startDate = $("#start-date-filtered-team").val();
    dueDate = $("#due-date-filtered-team").val();

    var divResult = $("#report-teams");
    divResult.html(LOADER);

    // Validation inputs.
    if (!validationDropdown($("#select-project-for-teams"))) return;
    if (!validationDropdown($("#select-employee-for-teams"))) return;
    if (!validationDropdown($("#select-activity-types"))) return;
    if (!validationDropdown($("#select-activity-statuses"))) return;
    if (!validationDropdown($("#start-date-filtered-team"))) return;
    if (!validationDropdown($("#due-date-filtered-team"))) return;

    // block button 
    button.setAttribute("disabled", "true");

    $.ajax({
        type: "POST",
        url: actionURL,
        traditional: true,
        data: {
            projectIds: projectIds,
            employeeIds: employeeIds,
            activityTypeIds: activityTypeIds,
            activityStatuses: activityStatuses,
            startDate: startDate,
            dueDate: dueDate
        },
        success: function (result) {
            divResult.html(result);
        },
        error: function () {
            divResult.html("Could not load data.");
        },
        complete: function () {
            button.removeAttribute("disabled");
        }
    });
}

// Generate projects filtered report and display on ui.
function generateProjectFilteredReport(button, actionURL) {

    projectIds = $("#select-project-for-activities").val();
    activityTypeIds = $("#select-activity-types").val();
    activityStatuses = $("#select-activity-statuses").val();
    startDate = $("#start-date-filtered-project").val();
    dueDate = $("#due-date-filtered-project").val();
    tableHeaders = $("#select-activity-headers").val();
    var divResult = $("#report-activities");
    divResult.html(LOADER);

    // Validation inputs.
    if (!validationDropdown($("#select-project-for-activities"))) return;
    if (!validationDropdown($("#select-activity-types"))) return;
    if (!validationDropdown($("#select-activity-statuses"))) return;
    if (!validationDropdown($("#start-date-filtered-project"))) return;
    if (!validationDropdown($("#due-date-filtered-project"))) return;
    if (!validationDropdown($("#select-activity-headers"))) return;

    


    // block button 
    button.setAttribute("disabled", "true");

    $.ajax({
        type: "POST",
        url: actionURL,
        traditional: true,
        data: {
            projectIds: projectIds,
            activityTypeIds: activityTypeIds,
            activityStatuses: activityStatuses,
            startDate: startDate,
            dueDate: dueDate,
            tableHeaders: tableHeaders
        },
        success: function (result) {
            divResult.html(result);
        },
        error: function () {
            divResult.html("Could not load data.");
        },
        complete: function () {
            button.removeAttribute("disabled");
        }
    });
}

// Generate sprints report and display on ui.
function generateSprintReport(button, actionURL) {
    sprintIds = $("#select-sprints");
    projectId = $("#select-project-for-sprints");
    divResult = $("#report-sprints");
    divResult.html(LOADER);

    // Validation inputs.
    if (!validationDropdown(sprintIds)) return;
    if (!validationDropdown(projectId)) return;

    // block button 
    button.setAttribute("disabled", "true");

    $.ajax({
        type: "POST",
        url: actionURL,
        traditional: true,
        data: {
            sprintIds: sprintIds.val(),
            projectId: projectId.val()
        },
        success: function (result) {
            divResult.html(result);
        },
        error: function () {
            divResult.html("Could not load data.");
        },
        complete: function () {
            button.removeAttribute("disabled");
        }
    });
}

// Generate users report and display on ui.
function generateUserReport(button, actionURL) {
    userIds = $("#select-employee");
    projectIds = $("#select-project");
    startDate = $("#emp-start-date");
    dueDate = $("#emp-due-date");
    divResult = $("#report-employee");
    divResult.html(LOADER);

    // Validation inputs.
    if (!validationRangeDate(startDate, dueDate)) return;
    if (!validationDropdown(userIds)) return;

    // block button 
    button.setAttribute("disabled", "true");

    $.ajax({
        type: "POST",
        url: actionURL,
        traditional: true,
        data: {
            userIds: userIds.val(),
            projectIds: projectIds.val(),
            startDate: startDate.val(),
            dueDate: dueDate.val()
        },
        success: function (result) {
            divResult.html(result);
        },
        error: function () {
            divResult.html("Could not load data.");
        },
        complete: function () {
            button.removeAttribute("disabled");
        }
    });
}

//download filtered teams report
function downloadFilteredTeamsReport(button, actionUrl, fileName) {
    debugger;
    projectIds = $("#select-project-for-teams").val();
    employeeIds = $("#select-employee-for-teams").val();
    activityTypeIds = $("#select-activity-types").val();
    activityStatuses = $("#select-activity-statuses").val();
    startDate = $("#start-date-filtered-team").val();
    dueDate = $("#due-date-filtered-team").val();
    tableHeaders = $("#select-team-headers").val();

    // Validation inputs.
    if (!validationDropdown($("#select-project-for-teams"))) return;
    if (!validationDropdown($("#select-employee-for-teams"))) return;
    if (!validationDropdown($("#select-activity-types"))) return;
    if (!validationDropdown($("#select-activity-statuses"))) return;
    if (!validationDropdown($("#start-date-filtered-team"))) return;
    if (!validationDropdown($("#due-date-filtered-team"))) return;
    if (!validationDropdown($("#select-team-headers"))) return;

    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            projectIds: projectIds,
            employeeIds: employeeIds,
            activityTypeIds: activityTypeIds,
            activityStatuses: activityStatuses,
            startDate: startDate,
            dueDate: dueDate,
            tableHeaders: tableHeaders
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName=" + fileName;
        },
        complete: function () {
            $(button).html($(button).data("original-text"));
        }
    });
}


//download filtered history report (for activities)
function downloadHistoryReport(button, actionUrl, fileName) {
    debugger;
    projectIds = $("#select-projects").val();
    assigneeIds = $("#select-teams").val();
    activityTypeIds = $("#select-activity-types").val();
    projectStatuses = $("#select-project-statuses").val();
    startDate = $("#start-date").val();
    dueDate = $("#due-date").val();
    intervalType = $("#select-period").val();
    tableHeaders = $("#select-history-headers").val();

    // Validation inputs.
    if (!validationDropdown($("#select-projects"))) return;
    if (!validationDropdown($("#select-teams"))) return;
    if (!validationDropdown($("#select-activity-types"))) return;
    if (!validationDropdown($("#select-project-statuses"))) return;
    if (!validationDropdown($("#start-date"))) return;
    if (!validationDropdown($("#due-date"))) return;
    if (!validationDropdown($("#select-period"))) return;
    if (!validationDropdown($("#select-history-headers"))) return;

    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            projectIds: projectIds,
            assigneeIds: assigneeIds,
            activityTypeIds: activityTypeIds,
            projectStatuses: projectStatuses,
            startDate: startDate,
            dueDate: dueDate,
            intervalType: intervalType,
            tableHeaders: tableHeaders
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName=" + fileName;
        },
        complete: function () {
            $(button).html($(button).data("original-text"));
        }
    });
}


// Generate history report and display on ui.
function generateHistoryReport(button, actionURL) {
    projectIds = $("#select-projects").val();
    assigneeIds = $("#select-teams").val();
    activityTypeIds = $("#select-activity-types").val();
    projectStatuses = $("#select-project-statuses").val();
    startDate = $("#start-date").val();
    dueDate = $("#due-date").val();
    intervalType = $("#select-period").val();
    tableHeaders = $("#select-history-headers").val();

    divResult = $("#report");
    divResult.html(LOADER);

    // Validation inputs.
    if (!validationDropdown($("#select-projects"))) return;
    if (!validationDropdown($("#select-teams"))) return;
    if (!validationDropdown($("#select-activity-types"))) return;
    if (!validationDropdown($("#select-project-statuses"))) return;
    if (!validationDropdown($("#start-date"))) return;
    if (!validationDropdown($("#due-date"))) return;
    if (!validationDropdown($("#select-history-headers"))) return;

    // block button 
    button.setAttribute("disabled", "true");

    $.ajax({
        type: "POST",
        url: actionURL,
        traditional: true,
        data: {
            projectIds: projectIds,
            assigneeIds: assigneeIds,
            activityTypeIds: activityTypeIds,
            projectStatuses: projectStatuses,
            startDate: startDate,
            dueDate: dueDate,
            intervalType: intervalType,
            tableHeaders: tableHeaders
        },
        success: function (result) {
            divResult.html(result);
        },
        error: function () {
            divResult.html("Could not load data.");
        },
        complete: function () {
            button.removeAttribute("disabled");
        }
    });
}

//dwonload general filtered report
function downloadGeneralReport(button, actionUrl, fileName) {
    debugger;
    var groupsIds = $("#select-project-group");
    var projectsIds = $("#select-projects");
    var teamsIds = $("#select-teams");
    var startDate = $("#start-date");
    var dueDate = $("#due-date");
    var tableHeaders = $("#select-table-headrs").val();

    // Validation inputs.
    if (!validationRangeDate(startDate, dueDate)) return;
    if (!validationDropdown(projectsIds)) return;

    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            groupsIds: groupsIds.val(),
            projectsIds: projectsIds.val(),
            teamsIds: teamsIds.val(),
            startDate: startDate.val(),
            dueDate: dueDate.val(),
            tableHeaders: tableHeaders
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName=" + fileName;
        },
        complete: function () {
            $(button).html($(button).data("original-text"));
        }
    });
}

//download filtered project report (for activities)
function downloadFilteredProjectReport(button, actionUrl, fileName) {
    debugger;
    projectIds = $("#select-project-for-activities").val();
    activityTypeIds = $("#select-activity-types").val();
    activityStatuses = $("#select-activity-statuses").val();
    startDate = $("#start-date-filtered-project").val();
    dueDate = $("#due-date-filtered-project").val();
    tableHeaders = $("#select-activity-headers").val();

    // Validation inputs.
    if (!validationDropdown($("#select-project-for-activities"))) return;
    if (!validationDropdown($("#select-activity-types"))) return;
    if (!validationDropdown($("#select-activity-statuses"))) return;
    if (!validationDropdown($("#start-date-filtered-project"))) return;
    if (!validationDropdown($("#due-date-filtered-project"))) return;
    if (!validationDropdown($("#select-activity-headers"))) return;


    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            projectIds: projectIds,
            activityTypeIds: activityTypeIds,
            activityStatuses: activityStatuses,
            startDate: startDate,
            dueDate: dueDate,
            tableHeaders: tableHeaders
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName=" + fileName;
        },
        complete: function () {
            $(button).html($(button).data("original-text"));
        }
    });
}

// download sprint report
function downloadSprintReport(button, actionUrl, fileName) {
    projectId = $("#select-project-for-sprints").val();
    sprintIds = $("#select-sprints").val();
    tableHeaders = $("#select-headers").val();

    // Validation inputs.
    if (!validationDropdown($("#select-project-for-sprints"))) return;
    if (!validationDropdown($("#select-sprints"))) return;
    if (!validationDropdown($("#select-headers"))) return;

    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            projectId: projectId,
            sprintIds: sprintIds,
            tableHeaders: tableHeaders
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName=" + fileName;
        },
        complete: function () {
            $(button).html($(button).data("original-text"));
        }
    });
}


// download user report.
function downloadUserReport(button, actionUrl, fileName) {
    userIds = $("#select-employee").val();
    projectIds = $("#select-project").val();
    startDate = $("#emp-start-date");
    dueDate = $("#emp-due-date");

    // Validation inputs.
    if (!validationRangeDate(startDate, dueDate)) return;
    if (!validationDropdown($("#select-employee"))) return;

    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            userIds: userIds,
            projectIds: projectIds,
            startDate: startDate.val(),
            dueDate: dueDate.val()
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName=" + fileName;
        },
        complete: function () {
            $(button).html($(button).data("original-text"));
        }
    });
}

// --------------------------------------------
// Generate and download project's logged time.
function downloadProjectReport(button, actionUrl, fileName) {
    var projectIds = $("#select-project").val();
    var startDate = $("#start-date");
    var dueDate = $("#due-date");

    // Validation inputs.
    if (!validationRangeDate(startDate, dueDate)) return;
    if (!validationDropdown($("#select-project"))) return;

    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            projectIds: projectIds,
            startDate: startDate.val(),
            dueDate: dueDate.val()
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName="+fileName;
        },
        complete: function() {
            $(button).html($(button).data("original-text"));
        }
    });
}

// -------------------------------------------
// Generate and download activity list report.
function downloadActivityListReport(button, actionUrl, fileName) {
    // Validation for drop down.
    if (!validationDropdown($("#select-activityList"))) return;

    const selectList = $("#select-activityList");

    $(button).html(btnLoader);

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: {
            activityListIds: selectList.val(),
            projectId: selectList.data("project-id")
        },
        success: function (result) {
            window.location.href = "/Reports/DownloadReport?fileName=" + fileName;
        },
        complete: function () {
            $(button).html($(button).data("original-text"));
        }
    });
}
