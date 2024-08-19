//document.addEventListener('DOMContentLoaded', (event) => {
//    // Function to close all forms
//    function closeAllForms() {
//        document.querySelectorAll('.form-container .collapse').forEach(form => {
//            form.classList.remove('show');
//            form.style.display = 'none';
//        });
//        document.querySelectorAll('.original-comment, .original-reply').forEach(comment => {
//            comment.style.display = 'block';
//        });
//    }

//    // Toggle edit forms
//    document.querySelectorAll('.edit-button').forEach(button => {
//        button.addEventListener('click', function (event) {
//            event.preventDefault();
//            closeAllForms();
//            const commentId = this.getAttribute('data-id');
//            const editForm = document.getElementById(`edit-form-${commentId}`);
//            const originalComment = document.getElementById(`original-comment-${commentId}`);
//            editForm.classList.add('show');
//            originalComment.style.display = 'none';
//        });
//    });

//    // Toggle delete forms
//    document.querySelectorAll('.delete-button').forEach(button => {
//        button.addEventListener('click', function (event) {
//            event.preventDefault();
//            closeAllForms();
//            const commentId = this.getAttribute('data-id');
//            const deleteForm = document.getElementById(`delete-form-${commentId}`);
//            const originalComment = document.getElementById(`original-comment-${commentId}`);
//            deleteForm.classList.add('show');
//            originalComment.style.display = 'none';
//        });
//    });

//    // Toggle reply forms
//    document.querySelectorAll('.reply-button').forEach(button => {
//        button.addEventListener('click', function (event) {
//            event.preventDefault();
//            closeAllForms();
//            const commentId = this.getAttribute('data-id');
//            const replyForm = document.getElementById(`create-form-${commentId}`);
//            const originalComment = document.getElementById(`original-comment-${commentId}`);
//            replyForm.classList.add('show');
//            originalComment.style.display = 'none';
//        });
//    });

//    // Cancel edit
//    document.querySelectorAll('.cancel-edit').forEach(button => {
//        button.addEventListener('click', function (event) {
//            event.preventDefault();
//            const commentId = this.getAttribute('data-id');
//            const editForm = document.getElementById(`edit-form-${commentId}`);
//            const originalComment = document.getElementById(`original-comment-${commentId}`);
//            editForm.classList.remove('show');
//            editForm.style.display = 'none';
//            originalComment.style.display = 'block';
//        });
//    });

//    // Cancel delete
//    document.querySelectorAll('.cancel-delete').forEach(button => {
//        button.addEventListener('click', function (event) {
//            event.preventDefault();
//            const commentId = this.getAttribute('data-id');
//            const deleteForm = document.getElementById(`delete-form-${commentId}`);
//            const originalComment = document.getElementById(`original-comment-${commentId}`);
//            deleteForm.classList.remove('show');
//            deleteForm.style.display = 'none';
//            originalComment.style.display = 'block';
//        });
//    });

//    // Cancel create
//    document.querySelectorAll('.cancel-create').forEach(button => {
//        button.addEventListener('click', function (event) {
//            event.preventDefault();
//            const commentId = this.getAttribute('data-id');
//            const createForm = document.getElementById(`create-form-${commentId}`);
//            const originalComment = document.getElementById(`original-comment-${commentId}`);
//            createForm.classList.remove('show');
//            createForm.style.display = 'none';
//            originalComment.style.display = 'block';
//        });
//    });
//});
