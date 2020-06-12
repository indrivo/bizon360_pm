var toolBar = [
    ['fontsize', ['fontsize']],
    ['fontname', ['fontname']],
    ['font', ['bold', 'italic', 'underline', 'clear']],
    ['color', ['color']],
    ['para', ['ul', 'ol', 'paragraph']],
    ['table', ['table']],
    ['insert', ['link']]
];

function Wiki() {

    this.SummerNote = function(id) {
        EnableSummerNote(id, toolBar);

        var editor = $(id);
        editor.summernote('foreColor', 'red');
        editor.summernote('fontName', 'Blinker');
        editor.summernote('fontSize', 14);
       // editor.summernote('removeFormat');
    }

    this.GetWikiByProject = function (projectId) {
        var self = this;
        $.ajax({
            type: "GET",
            url: "/Wiki/GetHeadlines",
            data: {
                projectId: projectId
            },
            success: function (result) {
                $("#headlines").html(result);
                self.OpenTab(projectId);
            }
        });
    }

    this.OpenTab = function(projectId)  {
        var tab = $("ul.wiki-tabs").children().children().attr("href");
        var savedTab = localStorage.getItem(projectId);

        if (savedTab !== null) {
            $('.nav-tabs a[href="' + savedTab + '"]').tab("show");
        } else {
            $('.nav-tabs a[href="' + tab + '"]').tab("show");
        }
    };

    this.SaveTab = function () {
        $("ul#headlines li a").click(function () {
        var $this = $(this);
        var key = $this.data("parent-id");
        var value = $this.attr("href");

        var checkIfExist = localStorage.getItem(key);

        if (checkIfExist === null || checkIfExist !== value) {
            localStorage.setItem(key, value);
        }
    })}
}