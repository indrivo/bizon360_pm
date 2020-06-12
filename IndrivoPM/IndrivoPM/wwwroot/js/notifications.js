var connection = new signalR.HubConnectionBuilder().withUrl("/gearHub").build();

connection.start().then(function () {
    //
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveNotification",
    function (message) {
        $.notificationAlert({
            title: message.message.title,
            message: message.message.body,
            time: message.sentTime,
           // url: returnUrl(message.messageRedirectAction)
        });

        notify.notifications.unshift({
            id: message.id,
            groupName: message.entityGroupName,
            title: message.message.title,
            from: message.from,
            body: message.message.body,
           // url: returnUrl(message.messageRedirectAction),
            sentTime: message.sentTime
        });

        $(".has-notifications").show();
    });

var btnLoader = ' <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';

// -----------------------------------------
// Vue.js container for render notifications.
var notify = new Vue({
    el: "#notify",
    data: {
        count: 0,
        groups: {},
        notifications: [{}]
    },
    methods: {
        // -------------
        // CRUD actions.
        deleteNotification: function (i, item) {
            $.ajax({
                type: "POST",
                url: "/Notifications/RemoveNotifications",
                data: { notificationIds: item.id },
                success: function () {

                    // Set next element like first element.
                    if ("canHide" in notify.groups[item.groupName].notifications[i]) {
                        if (notify.groups[item.groupName].notifications[i + 1]) {
                            notify.groups[item.groupName].notifications[i + 1].canHide = false;
                        }
                    }

                    // Remove item from array.
                    notify.groups[item.groupName].notifications.splice(i, 1);

                    notify.count--;
                    hasNotifications();
                }
            });
        },

        expandGroup: function (arr, groupName) {
            for (var i = 0; i < arr.length; i++) {
                if (!("canHide" in arr[i])) {
                    arr[i].display = !arr[i].display;
                }
            }

            notify.groups[groupName].isActive = !notify.groups[groupName].isActive;
        },

        clearGroup: function (event, groupName) {
            var ids = this.getIdsFromGroup(groupName);

            $(event.target).html(btnLoader);
            
            $.ajax({
                type: "POST",
                url: "/Notifications/RemoveNotifications",
                data: { notificationIds: ids },
                success: function () {
                    $(event.target).html($(event.target).data("original-name"));
                    notify.groups[groupName].notifications = [];
                    notify.count -= ids.length;
                    hasNotifications();
                }
            });
        },

        // Returns notification ids from group.
        getIdsFromGroup: function (groupName) {
            var notifications = this.groups[groupName].notifications;

            if (notifications.length === 0) return null;
            var ids = [];

            for (var i = 0; i < notifications.length; i++) {
                ids.push(notifications[i].id);
            }

            return ids;
        },

        isGroup: function (group) {
            var elementsInGroup = group.notifications.length;
            var isOpen = group.isActive;

            if (elementsInGroup > 1) {
                if (isOpen) {
                    return false;
                } else {
                    return true;
                }
            }
        }

    }
});


// -----------------------------------
// Gets from db notifications by email.
function GetNotifications(email) {
    $("#loader").html(LOADER_SM);
    $.ajax({
        type: "GET",
        url: "/Notifications/GetUserNotifications",
        data: { email: email },
        success: function (data) {
            $("#loader").remove();
            notify.count = data.length;
            notify.notifications = [{}];

            for (var i = 0; i < data.length; i++) {
                notify.notifications.push({
                    id: data[i].id,
                    groupName: data[i].entityGroupName,
                    title: data[i].message.title,
                    from: data[i].from,
                    body: data[i].message.body,
                    url: returnUrl(data[i].messageRedirectAction),
                    sentTime: data[i].sentTime,
                    display: false
                });
            }

            notify.groups = groupByProject(notify.notifications);
        }
    });
}

// -----------------------------------
// Returns complete redirect url string.
function returnUrl(redirectUrl) {
    if (redirectUrl == null || redirectUrl.controller == null) return null;

    var url = "/" + redirectUrl.controller;

    if (redirectUrl.action != null && redirectUrl.action !== "") {
        url += ("/" + redirectUrl.action);
    }

    if (redirectUrl.id != null && redirectUrl.id !== "") {
        url += ("/" + redirectUrl.id);
    }

    if (redirectUrl.additionalData != null && redirectUrl.additionalData !== "") {
        url += redirectUrl.additionalData;
    }

    return url;
}

// --------------------------------------
// Group array by project in a new object.
function groupByProject(arr) {
    var result = {};
    for (var i = 1; i < arr.length; i++) {

        var c = result[arr[i].groupName];

        if (!result[arr[i].groupName]) {
            result[arr[i].groupName] = {};
            result[arr[i].groupName].notifications = [];
            result[arr[i].groupName].isActive = false;
            arr[i].display = true;
            arr[i].canHide = false;
            result[arr[i].groupName].notifications.push(arr[i]);

        } else {
            result[arr[i].groupName].notifications.push(arr[i]);
        }
    }
    notify.count = (arr.length - 1);
    return result;
}

function hasNotifications() {
    var has = $(".has-notifications");

    if (notify.count > 0) {
        has.show();
    } else {
        has.hide();
    }
}

