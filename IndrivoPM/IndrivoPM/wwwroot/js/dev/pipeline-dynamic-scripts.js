// Stage Edit Form Loader
$.loadEditForm({
    bindTo: ".edit-stage",
    modalBody: ".edit-stage .modal-inner",
    ajaxUrl: "/Projects/RenameRecruitmentStage"
});

// Delete Confirmation for Project Groups
$.deleteConfirmation({
    bindTo: ".delete-stage",
    modalBody: ".modal-body.delete-stage",
    type: "group",
    inputId: "#stage-id"
});

// Project Edit Form Loader
$.loadEditForm({
    bindTo: ".edit-project-link",
    modalBody: ".edit-project .modal-inner",
    ajaxUrl: "/Projects/Edit"
});

// Delete Confirmation for Projects
$.deleteConfirmation({
    bindTo: ".delete-project-link",
    modalBody: ".modal-body.delete-project",
    type: "project",
    inputId: "#project-id"
});

// Display Switcher
$.bindCollapseToggle();

// Action Bindings
$.bindCreateProject();
$.bindMoveToGroupAction();
$.bindRenameProjectAction();
$.bindEditProjectStatus();
$.bindEditProjectMembers();