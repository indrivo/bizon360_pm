// Project Edit Form Loader
$.loadEditForm({
    bindTo: ".edit-candidate-link",
    modalBody: ".edit-candidate .modal-inner",
    ajaxUrl: "/Candidate/Edit"
});

// Delete Confirmation for Projects
$.deleteConfirmation({
    bindTo: ".delete-candidate-link",
    modalBody: ".modal-body.delete-candidate",
    type: "candidate",
    inputId: "#candidate-id"
});