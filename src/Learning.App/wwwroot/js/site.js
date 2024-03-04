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
