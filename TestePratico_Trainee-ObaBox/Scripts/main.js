$(document).ready(function () {
    $("#Uf").mask("SS").on("keyup blur", function () { $(this).val($(this).val().toUpperCase()) })
    $("#Cpf").mask("000.000.000-00")
    $("#Rg").mask("00.000-000")
    $("#Dt_nasc").val(moment($("#Dt_nasc").attr("value")).format("yyyy-MM-DD"))
    $("#Cep")
        .mask("00.000-000")
        .on("keyup blur", function () {
        $.ajax({
            url: `https://viacep.com.br/ws/${$(this).val().replace(/\D/g, '')}/json/`,
            method: "get",
            crossDomain: true,
        }).then(r => {
            if (!r.erro) {
                $("#Logradouro").val(r.logradouro)
                $("#Complemento").val(r.complemento)
                $("#Cidade").val(r.localidade)
                $("#Uf").val(r.uf)
            }
        })
    })

    $("select#clientesCadastrados").on("change", function () {
        if (this.value !== "") {
            $.ajax({
                url: '/Home/ListarEnderecos',
                method: 'get',
                data: { clienteId: this.value },
                dataType: 'json'
            }).then(({ data }) => {
                $("select#enderecosCadastrados option:not(:first)").remove()
                $("select#enderecosCadastrados option:first").text("- Selecione o Endereço")

                data.forEach(v => $("select#enderecosCadastrados").append(`<option value='${v.Id}'>${v.Logradouro} ${v.Numero}</option>`))
            })
        } else {
            $("select#enderecosCadastrados option:not(:first)").remove()
            $("select#enderecosCadastrados option:first").text("- Selecione o Cliente")
        }
    })

    $(".dateModifier").each((k, v) => {
        v.innerHTML = moment(v.innerHTML).format("DD/MM/YYYY hh:mm:ss A");
    })

    $("button[deletarCliente]").on("click", function () {
        const row = $(this).parents("tr")
        Swal.fire({
            title: 'Deseja realmente executar essa ação?',
            icon: 'question',
            html: `<p><i>Confirme os dados antes de deletar</i>:</p>
            <ul class="listaDelete">
                <li><b>ID</b>: ${$(row).find("td[data-name='id']").html()}</li>
                <li><b>Nome</b>: ${$(row).find("td[data-name='nome']").html()}</li>
                <li><b>CPF</b>: ${$(row).find("td[data-name='cpf']").html()}</li>
                <li><b>RG</b>: ${$(row).find("td[data-name='rg']").html()}</li>
                <li><b>Data de Nascimento</b>: ${$(row).find("td[data-name='dt_nasc']").html()}</li>
                <li><b>Ativo</b>: ${$(row).find("td[data-name='ativo']").html()}</li>
            </ul>
            `,
            showDenyButton: true,
            confirmButtonText: 'Sim, deletar.',
            denyButtonText: 'Não, cancelar.',
        }).then(r => {
            if (r.isDismissed || r.isDenied)
                return Swal.fire('Tudo certo!', 'O registro não foi excluído ;)', 'success')
            else if (r.isConfirmed) {
                $.ajax({
                    url: "/Home/DeletarCliente/",
                    type: "post",
                    data: { id: $(row).find("td[data-name='id']").html() }
                })
                    .then(r => Swal.fire(r.title, r.text, r.icon).then(() => location.reload()))
            }
        })
    })
    $("button[deletarEndereco]").on("click", function () {
        const row = $(this).parents("tr")
        Swal.fire({
            title: 'Deseja realmente executar essa ação?',
            icon: 'question',
            html: `<p><i>Confirme os dados antes de deletar</i>:</p>
            <ul class="listaDelete">
                <li><b>ID</b>: ${$(row).find("td[data-name='id']").html()}</li>
                <li><b>Cliente</b>: ${$(row).find("td[data-name='cliente']").html()}</li>
                <li><b>Endereço</b>: ${$(row).find("td[data-name='endereco']").html()}</li>
            </ul>
            `,
            showDenyButton: true,
            confirmButtonText: 'Sim, deletar.',
            denyButtonText: 'Não, cancelar.',
        }).then(r => {
            if (r.isDismissed || r.isDenied)
                return Swal.fire('Tudo certo!', 'O registro não foi excluído ;)', 'success')
            else if (r.isConfirmed) {
                $.ajax({
                    url: "/Home/DeletarEndereco/",
                    type: "post",
                    data: { id: $(row).find("td[data-name='id']").html() }
                })
                    .then(r => Swal.fire(r.title, r.text, r.icon).then(() => location.reload()))
            }
        })
    })
    $("button[deletarCompra]").on("click", function () {
        const row = $(this).parents("tr")
        Swal.fire({
            title: 'Deseja realmente executar essa ação?',
            icon: 'question',
            html: `<p><i>Confirme os dados antes de deletar</i>:</p>
            <ul class="listaDelete">
                <li><b>Loja</b>: ${$(row).find("td[data-name='loja']").html()}</li>
                <li><b>Cliente</b>: ${$(row).find("td[data-name='cliente']").html()}</li>
                <li><b>Endereço</b>: ${$(row).find("td[data-name='endereco']").html()}</li>
            </ul>
            `,
            showDenyButton: true,
            confirmButtonText: 'Sim, deletar.',
            denyButtonText: 'Não, cancelar.',
        }).then(r => {
            if (r.isDismissed || r.isDenied)
                return Swal.fire('Tudo certo!', 'O registro não foi excluído ;)', 'success')
            else if (r.isConfirmed) {
                $.ajax({
                    url: "/Home/DeletarCompra/",
                    type: "post",
                    data: {
                        idLoja: $(row).data("idloja"),
                        idCliente: $(row).data("idcliente"),
                        idEndereco: $(row).data("idendereco"),
                    }
                })
                    .then(r => Swal.fire(r.title, r.text, r.icon).then(() => location.reload()))
            }
        })
    })
})