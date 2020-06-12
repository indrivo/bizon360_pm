window.entity = {
    ids: null,
    parentIds: null
};

function BulkContainer() {

    var action;
    var controller;


    $(".bulk-check").change(function() {
        entity.parentIds = $(this).data("parent-id");
    });

    this.ShowBulkContainer = function() {
        $(".bulk-actions-container").addClass("show");
        $(".bulk-hidden").removeClass("d-none");
    }

    this.HideBulkContainer = function() {
        $(".bulk-actions-container").removeClass("show");
        $(".bulk-hidden").addClass("d-none");
    }

    this.DeselectAllActivities = function () {

        var self = this;

        $("#deselect-all").click(function() {
            $(".bulk-check").prop("checked", false);
            $(".check-all").prop("checked", false);
            self.HideBulkContainer();
            $("#check-counter").html(self.GetCheckedCount());
        });
    }

    this.DeselectAll = function () {

        $(".bulk-check").prop("checked", false);
        $(".check-all").prop("checked", false);
        this.HideBulkContainer();
        $("#check-counter").html(this.GetCheckedCount());
        
    }

    this.GetCheckedCount = function() {
        return $(".bulk-check:checked").length;
    }

    this.GetChecked = function() {
        var checked = [];

        $(".bulk-check:checked").each(function() {
            checked.push($(this).attr("id"));
        });

        return checked;
    }

    this.CheckAll = function () {
        var self = this;

        $(".check-all").click(function () {
            var $this = $(this);
            var childCheckboxes = $this.closest("table").find(".bulk-check")

            if ($this.is(":checked")) {
                childCheckboxes.prop("checked", true);
                self.ShowBulkContainer();
                $("#check-counter").html(self.GetCheckedCount());
            } else {
                childCheckboxes.prop("checked", false);
                self.HideBulkContainer();
            }
        });
    }

    this.Enable = function () {

        var self = this;

        this.CheckAll();

        this.DeselectAllActivities();

        $(".bulk-check").click(function() {

            $("#check-counter").html(self.GetCheckedCount());

            if ($(this).prop("checked") === true) {
                self.ShowBulkContainer();
            } else if (self.GetCheckedCount() === 0) {
                self.HideBulkContainer();
            }
        });

        $(".map-content").click(function () {

            entity.parentIds = $(this).data("id");

            var mapContent = $("#show-" + entity.parentIds);

            if (mapContent.data("opened") === false) {
                mapContent.data("opened", true);

                self.GetEntityByParent("/Services/GetTrackersByActivity/");
            }
        });
    }

    this.GetCheckedMapIds = function () {
        var c = [];

        $(".content-map").has(".bulk-check:checked").each(function() {
            c.push($(this).attr("id").replace("show-", ""));
        });
            
       return c;
    }

    this.RefreshMap = function (actionUrl) {
        var mapIds = this.GetCheckedMapIds();

        for (var i = 0; i < mapIds.length; i++) {
            this.GetEntityByParent(actionUrl, mapIds[i]);
        }
    }

    // Get entities from another entity
    // --------------------------------
    this.GetEntityByParent = function (actionUrl, parentId = entity.parentIds) {
        var self = this;
        $.ajax({
            url: actionUrl,
            type: "GET",
            data: {
                id: parentId,
                searchField: $(".search-input").val()
            },
            success: function (result) {

                $("#show-" + parentId).html(result);

                if ($("#show-" + parentId + " tr").length !== 0) {
                    $("#map-count-" + parentId).html($("#show-" + parentId + " tr").length - 1);
                } else {
                    $("#map-count-" + parentId).html("0");
                }

            },
            error: function() {
                $("#show-" + parentId)
                    .html(
                        "<div class=\"text-center\"><i data-feather=\"frown\"></i></div><h5 class=\"text-center mt-2\">Something went wrong</h5>");
            }
        });
    }

    // On click create, show modal
    // ---------------------------
    this.FormModal = function(options) {
        var self = this;

        $(options.bindTo).click(function() {

            var url = "/" + $(this).data("controller") + "/" + $(this).data("action");

            $("#modal-generic .modal-inner").html(LOADER);
            $("#modal-generic .modal-title").html(options.modalName);
            $("#modal-generic .modal-footer").addClass("d-none");
            $("#modal-generic .modal-inner").removeClass("p-3");

            $.ajax({
                url: url,
                type: "POST",
                data: { checkedIds: self.GetChecked() },
                success: function(result) {
                    $("#modal-generic .modal-inner").html(result);
                    if (options.eventFunction != null) {
                        options.eventFunction(result);
                    }
                },
                error: function(result) {
                    ErrorAlert("Could not load data");
                }
            });
        });
    }


    // Modal for confirmation some action
    // ----------------------------------------
    this.ComfirmationModal = function(options) {
        var self = this;

        $(options.bindTo).click(function() {
            var $this = $(this);

            var rowId = $this.data("id");

            if (rowId == null || rowId === "undefined") {
                entity.ids = self.GetChecked();
            } else {
                entity.ids = rowId;
            }

            if (entity.ids.length === 0) {
                entity.ids = $this.data("id");
            }

            controller = $this.data("controller");
            action = $this.data("action");

            console.log(entity.ids);

            var btnSuccess = '<button type = "button" class= "btn btn-danger-custom font-weight-600" id = "' +
                options.confirmBtnId +
                '" >' +
                options.confirmBtnName +
                '</button >';

            if (options.modalId == null) {
                options.modalId = "#modal-generic";
            }

            $(options.modalId + " .modal-footer").removeClass('d-none');
            $(options.modalId + " .modal-footer").html(btnSuccess);

            $(options.modalId + " .modal-inner").addClass("p-3");
            $(options.modalId + " .modal-inner").html(options.messageConfirmation);
            $(options.modalId + " .modal-title").html(options.modalName);
            $(options.modalId).modal("show");
        });
    }

    // On click confirm action
    // ----------------------------
    this.SaveChanges = function (options) {
        var self = this;
        $(options.confirmBtn).click(function () {

            $(options.confirmBtn).prop('disabled', true);

            console.log(controller + "/" + action + "/" + entity.ids);

            $.ajax({
                type: "POST",
                url: "/" + controller + "/" + action,
                data: { ids: entity.ids },
                error: function() {
                },
                success: function (result) {
                    if (options.eventFunction != null)
                        options.eventFunction(result);
                    SuccessAlert(options.successMessage);
                    $("#modal-generic").modal("hide");
                    $("#modal-generic .modal-footer").addClass("d-none");
                    self.DeselectAll();

                }
            });
        });
    }
}
