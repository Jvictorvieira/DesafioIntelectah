// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showSuccessToast(message) {
    var toastBody = document.getElementById('successToastBody');
    toastBody.textContent = message;
    var toastEl = document.getElementById('successToast');
    var toast = new bootstrap.Toast(toastEl);
    toast.show();
}

function showErrorToast(message) {
    var toastBody = document.getElementById('errorToastBody');
    toastBody.textContent = message;
    var toastEl = document.getElementById('errorToast');
    var toast = new bootstrap.Toast(toastEl);
    toast.show();
}

function openEntityModal(url, title) {
    fetch(url)
        .then(response => response.text())
        .then(html => {
            document.getElementById('entityModalBody').innerHTML = html;
            document.getElementById('entityModalLabel').textContent = title;
            var modal = new bootstrap.Modal(document.getElementById('entityModal'));
            modal.show();

            
            var modalForm = document.getElementById('modalForm');
            if (modalForm) {
                modalForm.onsubmit = function(e) {
                    e.preventDefault();
                    var url = modalForm.action;
                    var formData = new FormData(modalForm);

                    fetch(url, {
                        method: 'POST',
                        body: formData
                    })
                    .then(response => response.text())
                    .then(htmlOrJson => {
                        try {
                            var result = JSON.parse(htmlOrJson);
                            if (result.success) {
                                modal.hide();
                                showSuccessToast(result.message);
                            }
                        } catch {
                            document.getElementById('entityModalBody').innerHTML = htmlOrJson;
                        }
                    });
                };
            }
    });
}

function openDeleteModal(url, title) {
    fetch(url)
        .then(response => response.text())
        .then(html => {
            document.getElementById('deleteModalBody').innerHTML = html;
            document.getElementById('deleteModalLabel').textContent = title;
        var modal = new bootstrap.Modal(document.getElementById('deleteModal'));
        modal.show();

        
        var deleteForm = document.getElementById('deleteForm');
        if (deleteForm) {
            deleteForm.onsubmit = function(e) {
                e.preventDefault();
                var url = deleteForm.action;
                var formData = new FormData(deleteForm);

                fetch(url, {
                    method: 'POST',
                    body: formData
                })
                .then(response => response.text())
                .then(htmlOrJson => {
                    try {
                        var result = JSON.parse(htmlOrJson);
                        if (result.success) {
                            modal.hide();
                            showSuccessToast(result.message);
                        }
                    } catch {
                        document.getElementById('deleteModalBody').innerHTML = htmlOrJson;
                    }
                });
            };
        }
    });
}
    
