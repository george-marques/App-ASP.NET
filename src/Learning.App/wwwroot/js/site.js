function AjaxModal() {
    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on("click", function (e) {
                e.preventDefault(); // Evita a ação padrão do link
                $('#myModalContent').load(this.href, function () {
                    $('#myModal').modal('show'); // Abre o modal
                    bindForm(this);
                });
            });
        });

        function bindForm(dialog) {
            $(dialog).on('click', '.modal-header .close', function () {
                $('#myModal').modal('hide'); // Fecha o modal quando o botão "X" é clicado
            });

            $(dialog).on('click', '.modal-footer button[data-dismiss="modal"]', function () {
                $('#myModal').modal('hide'); // Fecha o modal quando o botão "Fechar" é clicado
            });

            $(dialog).on('click', 'button[type="submit"]', function () {
                var form = $(this).closest('form');
                $.ajax({
                    url: form.attr('action'),
                    type: form.attr('method'),
                    data: form.serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#myModal').modal('hide');
                            $('#EnderecoTarget').load(result.url);
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                });
                return false;
            });
        }
    });
}

function SearchPostalCode() {
    $(document).ready(function () {

        function clearPostalCodeForm() {
            $('#Endereco_Logradouro').val('');
            $('#Endereco_Bairro').val('');
            $('#Endereco_Cidade').val('');
            $('#Endereco_Estado').val('');
        }

        $('#Endereco_Cep').blur(function () {

            var postalCode = $(this).val().replace(/\D/g, '');

            if (postalCode != '') {

                var validatePostalCode = /^[0-9]{8}$/;

                if (validatePostalCode.test(postalCode)) {

                    $('#Endereco_Logradouro').val('...');
                    $('#Endereco_Bairro').val('...');
                    $('#Endereco_Cidade').val('...');
                    $('#Endereco_Estado').val('...');

                    $.getJSON('https://viacep.com.br/ws/' + postalCode + '/json/?callback=?',
                        function (data) {

                            if (!('erro' in data)) {
                                $('#Endereco_Logradouro').val(data.logradouro);
                                $('#Endereco_Bairro').val(data.bairro);
                                $('#Endereco_Cidade').val(data.localidade);
                                $('#Endereco_Estado').val(data.uf);
                            }
                            else {
                                clearPostalCodeForm();
                                alert('CEP não encontrado.');
                            }
                        });
                }
                else {
                    clearPostalCodeForm();
                    alert('Formato de CEP inválido');
                }
            }
            else {
                clearPostalCodeForm();
            }
        });
    });
}

$(document).ready(function () {
    $('#msg_box').fadeOut(2500);
});