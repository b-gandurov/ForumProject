$(document).ready(function () {
    $('[data-bs-toggle="tooltip"]').tooltip({
        html: true
    });
});

tinymce.init({
    selector: 'textarea',
    height: 200,
    plugins: 'autolink link image lists print preview',
    toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright | bullist numlist outdent indent | link image',
    forced_root_block: '',
    setup: function (editor) {
        editor.on('GetContent', function (e) {
            e.content = e.content.replace(/<\/?p[^>]*>/g, '');
        });
    }
});
