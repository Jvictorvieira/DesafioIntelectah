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

            

            processModalForm(modal);

            
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
                            getTableData(result.url);
                        }
                    } catch {
                        document.getElementById('deleteModalBody').innerHTML = htmlOrJson;
                    }
                });
            };
        }
    });
}
    
function getTableData(url) {
    fetch(url)
        .then(response => response.text())
        .then(html => {
            document.getElementById('data-table').innerHTML = html;
        });
}

function setFieldError(fieldName, message) {
    var field = document.querySelector(`[name='${fieldName}']`);
    if (field) {
        // Adiciona classe de erro
        field.classList.add('is-invalid');
        // Cria ou atualiza o span de mensagem
        let errorSpan = field.parentElement.querySelector('.field-validation-error');
        if (!errorSpan) {
            errorSpan = document.createElement('span');
            errorSpan.className = 'field-validation-error text-danger';
            field.parentElement.appendChild(errorSpan);
        }
        errorSpan.textContent = message;
    }
}

function clearFieldError(fieldName) {
    var field = document.querySelector(`[name='${fieldName}']`);
    if (field) {
        field.classList.remove('is-invalid');
        let errorSpan = field.parentElement.querySelector('.field-validation-error');
        if (errorSpan) errorSpan.textContent = '';
    }
}

async function validateCep(cep) {
    const response = await fetch(`https://viacep.com.br/ws/${cep}/json`);
    const data = await response.json();
    if (data.erro) {
        setFieldError('AddressCode', 'CEP inválido!');
        
    }

    // Atualiza os campos do formulário
    document.querySelector('[name="Address"]').value = data.logradouro || '';
    document.querySelector('[name="City"]').value = data.localidade || '';
    document.querySelector('[name="State"]').value = data.estado || data.uf || '';


}

function initCepMask() {
    var cepInput = document.getElementById('AddressCode');
    if (cepInput) {
        cepInput.addEventListener('input', function() {
            //Mascara CEP
            let v = cepInput.value.replace(/\D/g, '');
            if (v.length > 5) v = v.replace(/^(\d{5})(\d)/, '$1-$2');
            cepInput.value = v;

            if (v.replace('-', '').length === 8) {
                validateCep(v.replace('-', ''));
            }
        });
    }
};

function initPhoneMask() {
    var phoneInput = document.getElementById('Phone');
    if (phoneInput) {
        phoneInput.addEventListener('input', function() {
            let v = phoneInput.value.replace(/\D/g, '');
            if (v.length <= 10) {
                // Formato (99) 9999-9999
                v = v.replace(/^(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
            } else {
                // Formato (99) 99999-9999
                v = v.replace(/^(\d{2})(\d{5})(\d{0,4})/, '($1) $2-$3');
            }
            phoneInput.value = v.trim().replace(/[-\s]+$/, '');
        });
    }
}
function initCpfMask(){
    var cpfInput = document.getElementById('Cpf');
    if (cpfInput) {
        cpfInput.addEventListener('input', function() {
            let v = cpfInput.value.replace(/\D/g, '');
            if (v.length > 11) v = v.slice(0, 11);
            if (v.length > 9) {
                // Formato 999.999.999-99
                v = v.replace(/^(\d{3})(\d{3})(\d{0,2})/, '$1.$2-$3');
            } else if (v.length > 6) {
                // Formato 999.999-99
                v = v.replace(/^(\d{3})(\d{3})(\d{0,2})/, '$1.$2-$3');
            } else if (v.length > 3) {
                // Formato 999-99
                v = v.replace(/^(\d{3})(\d{0,2})/, '$1-$2');
            }
            cpfInput.value = v.trim().replace(/[-\s]+$/, '');
        });
    }
}

function processModalForm(modal) {
    initCepMask();
    initPhoneMask();
    var modalForm = document.getElementById('modalForm');
    if (modalForm) {
        modalForm.onsubmit = function (e) {
            e.preventDefault();
            var url = modalForm.action;
            var formData = new FormData(modalForm);

            clearFieldError('Cep');

            fetch(url, {
                method: 'POST',
                body: formData
            })
                .then(response => response.text())
                .then(htmlOrJson => {
                    try {
                        var result = JSON.parse(htmlOrJson);
                        if (result.success) {
                            debugger
                            modal.hide();
                            showSuccessToast(result.message);
                            getTableData(result.url);
                        }
                    } catch {
                        debugger
                        document.getElementById('entityModalBody').innerHTML = htmlOrJson;
                        processModalForm(modal);
                    }
                });
        };
    }
}