const API_STORAGE_KEY = "medi_api_base_url";
const AUTH_STORAGE_KEY = "medi_admin_auth";

const defaultApiBase = "http://localhost:5047/api";

const schema = {
  Doctores: {
    endpoint: "Doctores",
    fields: [
      { name: "nombre", label: "Nombre", type: "text", required: true },
      { name: "apellido", label: "Apellido", type: "text", required: true },
      { name: "cedula", label: "Cedula", type: "text", required: true },
      { name: "sexo", label: "Sexo", type: "select", required: true, options: ["Masculino", "Femenino"] },
      { name: "especialidad", label: "Especialidad", type: "text", required: true },
      { name: "telefono", label: "Telefono", type: "text", required: true },
      { name: "direccion", label: "Direccion", type: "text", required: true }
    ]
  },
  Pacientes: {
    endpoint: "Pacientes",
    fields: [
      { name: "nombre", label: "Nombre", type: "text", required: true },
      { name: "apellido", label: "Apellido", type: "text", required: true },
      { name: "cedula", label: "Cedula", type: "text", required: true },
      { name: "telefono", label: "Telefono", type: "text", required: true },
      { name: "email", label: "Email", type: "email", required: true },
      { name: "fechaNacimiento", label: "Fecha Nacimiento", type: "date", required: true },
      { name: "direccion", label: "Direccion", type: "text", required: true }
    ]
  },
  Tratamientos: {
    endpoint: "Tratamientos",
    fields: [
      { name: "nombre", label: "Nombre", type: "text", required: true },
      { name: "descripcion", label: "Descripcion", type: "textarea", required: true },
      { name: "costo", label: "Costo", type: "number", required: true, step: "0.01", min: "0" }
    ]
  },
  Sesiones: {
    endpoint: "Sesiones",
    fields: [
      { name: "fechaHora", label: "Fecha y Hora", type: "datetime-local", required: true },
      { name: "duracionMinutos", label: "Duracion (min)", type: "number", required: true, min: "1", max: "1440" },
      { name: "notas", label: "Notas", type: "textarea", required: false },
      { name: "pacienteId", label: "Paciente", type: "entity-select", required: true, source: "Pacientes", optionLabel: (x) => `${x.nombre} ${x.apellido}` },
      { name: "doctorId", label: "Doctor", type: "entity-select", required: true, source: "Doctores", optionLabel: (x) => `Dr. ${x.nombre} ${x.apellido}` },
      { name: "tratamientoId", label: "Tratamiento", type: "entity-select", required: true, source: "Tratamientos", optionLabel: (x) => x.nombre }
    ]
  }
};

const state = {
  activeEntity: "Doctores",
  editingId: null,
  auth: null,
  records: [],
  lookups: {
    Doctores: [],
    Pacientes: [],
    Tratamientos: []
  }
};

const el = {
  apiBaseUrl: document.getElementById("apiBaseUrl"),
  registerForm: document.getElementById("registerForm"),
  loginForm: document.getElementById("loginForm"),
  logoutBtn: document.getElementById("logoutBtn"),
  sessionName: document.getElementById("sessionName"),
  sessionRole: document.getElementById("sessionRole"),
  tabs: document.getElementById("tabs"),
  formTitle: document.getElementById("formTitle"),
  entityForm: document.getElementById("entityForm"),
  tableTitle: document.getElementById("tableTitle"),
  tableHead: document.getElementById("tableHead"),
  tableBody: document.getElementById("tableBody"),
  refreshBtn: document.getElementById("refreshBtn"),
  toast: document.getElementById("toast")
};

init();

function init() {
  restoreApiBase();
  restoreAuth();
  renderTabs();
  bindEvents();
  refreshSessionUI();
  renderEntityView();

  if (state.auth?.token) {
    loadEntityData();
  }
}

function bindEvents() {
  el.apiBaseUrl.addEventListener("change", () => {
    localStorage.setItem(API_STORAGE_KEY, normalizeApiBase(el.apiBaseUrl.value));
    toast("API base guardada.", "ok");
  });

  el.registerForm.addEventListener("submit", onRegister);
  el.loginForm.addEventListener("submit", onLogin);
  el.logoutBtn.addEventListener("click", onLogout);
  el.refreshBtn.addEventListener("click", () => loadEntityData());
}

function restoreApiBase() {
  const fromStorage = localStorage.getItem(API_STORAGE_KEY);
  el.apiBaseUrl.value = fromStorage || defaultApiBase;
}

function normalizeApiBase(value) {
  return String(value || "").trim().replace(/\/$/, "");
}

function getApiBase() {
  return normalizeApiBase(el.apiBaseUrl.value || defaultApiBase);
}

function restoreAuth() {
  try {
    const raw = localStorage.getItem(AUTH_STORAGE_KEY);
    state.auth = raw ? JSON.parse(raw) : null;
  } catch {
    state.auth = null;
  }
}

function persistAuth(auth) {
  state.auth = auth;
  if (auth) {
    localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(auth));
  } else {
    localStorage.removeItem(AUTH_STORAGE_KEY);
  }
  refreshSessionUI();
}

function refreshSessionUI() {
  if (!state.auth) {
    el.sessionName.textContent = "No autenticado";
    el.sessionRole.textContent = "-";
    return;
  }

  el.sessionName.textContent = `${state.auth.nombre} (${state.auth.username})`;
  el.sessionRole.textContent = state.auth.rol;
}

function renderTabs() {
  el.tabs.innerHTML = "";
  Object.keys(schema).forEach((entity) => {
    const btn = document.createElement("button");
    btn.type = "button";
    btn.textContent = entity;
    btn.classList.toggle("active", entity === state.activeEntity);
    btn.addEventListener("click", () => {
      state.activeEntity = entity;
      state.editingId = null;
      renderTabs();
      renderEntityView();
      if (state.auth?.token) {
        loadEntityData();
      }
    });
    el.tabs.appendChild(btn);
  });
}

function renderEntityView() {
  const config = schema[state.activeEntity];
  el.formTitle.textContent = `Formulario ${state.activeEntity}`;
  el.tableTitle.textContent = `Listado ${state.activeEntity}`;

  renderEntityForm(config);
  renderTable([], config);
}

function renderEntityForm(config) {
  el.entityForm.innerHTML = "";

  const idInput = createInput({
    name: "id",
    type: "hidden",
    required: false
  });
  el.entityForm.appendChild(idInput);

  config.fields.forEach((field) => {
    const wrapper = document.createElement("div");

    const label = document.createElement("label");
    label.textContent = field.label;
    label.setAttribute("for", field.name);

    const input = createInput(field);
    input.id = field.name;

    wrapper.appendChild(label);
    wrapper.appendChild(input);
    el.entityForm.appendChild(wrapper);
  });

  const actionWrap = document.createElement("div");
  actionWrap.style.display = "flex";
  actionWrap.style.gap = "8px";

  const submit = document.createElement("button");
  submit.type = "submit";
  submit.textContent = "Guardar";

  const clearBtn = document.createElement("button");
  clearBtn.type = "button";
  clearBtn.className = "ghost";
  clearBtn.textContent = "Limpiar";
  clearBtn.addEventListener("click", clearForm);

  actionWrap.appendChild(submit);
  actionWrap.appendChild(clearBtn);

  el.entityForm.appendChild(actionWrap);
  el.entityForm.onsubmit = onSubmitEntity;
}

function createInput(field) {
  let input;

  if (field.type === "textarea") {
    input = document.createElement("textarea");
    input.rows = 3;
  } else if (field.type === "select" || field.type === "entity-select") {
    input = document.createElement("select");
  } else {
    input = document.createElement("input");
    input.type = field.type || "text";
    if (field.step) input.step = field.step;
    if (field.min) input.min = field.min;
    if (field.max) input.max = field.max;
  }

  input.name = field.name;
  if (field.required) input.required = true;

  if (field.type === "select") {
    field.options.forEach((value) => {
      const option = document.createElement("option");
      option.value = value;
      option.textContent = value;
      input.appendChild(option);
    });
  }

  if (field.type === "entity-select") {
    const placeholder = document.createElement("option");
    placeholder.value = "";
    placeholder.textContent = "Seleccionar";
    input.appendChild(placeholder);
  }

  return input;
}

async function onRegister(event) {
  event.preventDefault();

  const form = event.currentTarget;
  const body = {
    nombre: form.nombre.value.trim(),
    username: form.username.value.trim(),
    email: form.email.value.trim(),
    password: form.password.value,
    rol: form.rol.value
  };

  try {
    const auth = await apiFetch("Auth/register", {
      method: "POST",
      body
    });

    persistAuth(mapAuth(auth));
    form.reset();
    toast("Registro completado. Sesion iniciada.", "ok");
    if (state.auth.rol !== "Admin") {
      toast("Solo rol Admin puede usar CRUD en este front.", "error");
      return;
    }
    await loadEntityData();
  } catch (error) {
    toast(error.message, "error");
  }
}

async function onLogin(event) {
  event.preventDefault();
  const form = event.currentTarget;

  const body = {
    usernameOrEmail: form.usernameOrEmail.value.trim(),
    password: form.password.value
  };

  try {
    const auth = await apiFetch("Auth/login", {
      method: "POST",
      body
    });

    persistAuth(mapAuth(auth));
    form.reset();
    toast("Login correcto.", "ok");

    if (state.auth.rol !== "Admin") {
      toast("Este panel esta pensado para usuarios Admin.", "error");
      return;
    }

    await loadEntityData();
  } catch (error) {
    toast(error.message, "error");
  }
}

function onLogout() {
  persistAuth(null);
  clearForm();
  renderTable([], schema[state.activeEntity]);
  toast("Sesion cerrada.", "ok");
}

async function loadEntityData() {
  if (!requireAdmin()) return;

  const config = schema[state.activeEntity];

  try {
    if (state.activeEntity === "Sesiones") {
      await Promise.all([
        loadLookup("Doctores"),
        loadLookup("Pacientes"),
        loadLookup("Tratamientos")
      ]);
      patchSessionSelects();
    }

    state.records = await apiFetch(config.endpoint);
    renderTable(state.records, config);
  } catch (error) {
    renderTable([], config);
    toast(error.message, "error");
  }
}

async function loadLookup(entityName) {
  state.lookups[entityName] = await apiFetch(schema[entityName].endpoint);
}

function patchSessionSelects() {
  const sessionFields = schema.Sesiones.fields.filter((x) => x.type === "entity-select");

  sessionFields.forEach((field) => {
    const select = el.entityForm.elements[field.name];
    if (!select) return;

    select.innerHTML = "";

    const placeholder = document.createElement("option");
    placeholder.value = "";
    placeholder.textContent = "Seleccionar";
    select.appendChild(placeholder);

    state.lookups[field.source].forEach((item) => {
      const option = document.createElement("option");
      option.value = item.id;
      option.textContent = `${item.id} - ${field.optionLabel(item)}`;
      select.appendChild(option);
    });
  });
}

function renderTable(records, config) {
  const columns = ["id", ...config.fields.map((f) => f.name)];

  el.tableHead.innerHTML = "";
  const hr = document.createElement("tr");
  columns.forEach((c) => {
    const th = document.createElement("th");
    th.textContent = humanize(c);
    hr.appendChild(th);
  });
  const actionsTh = document.createElement("th");
  actionsTh.textContent = "Acciones";
  hr.appendChild(actionsTh);
  el.tableHead.appendChild(hr);

  el.tableBody.innerHTML = "";

  if (!records.length) {
    const tr = document.createElement("tr");
    const td = document.createElement("td");
    td.colSpan = columns.length + 1;
    td.className = "empty";
    td.textContent = "Sin registros para mostrar";
    tr.appendChild(td);
    el.tableBody.appendChild(tr);
    return;
  }

  records.forEach((row) => {
    const tr = document.createElement("tr");

    columns.forEach((c) => {
      const td = document.createElement("td");
      td.textContent = formatCell(row[c], c);
      tr.appendChild(td);
    });

    const actions = document.createElement("td");
    actions.className = "actions";

    const editBtn = document.createElement("button");
    editBtn.type = "button";
    editBtn.textContent = "Editar";
    editBtn.addEventListener("click", () => fillForm(row));

    const deleteBtn = document.createElement("button");
    deleteBtn.type = "button";
    deleteBtn.className = "delete-btn";
    deleteBtn.textContent = "Eliminar";
    deleteBtn.addEventListener("click", () => deleteEntity(row.id));

    actions.appendChild(editBtn);
    actions.appendChild(deleteBtn);
    tr.appendChild(actions);

    el.tableBody.appendChild(tr);
  });
}

function formatCell(value, key) {
  if (value === null || value === undefined || value === "") return "-";
  if (key.toLowerCase().includes("fecha") && String(value).includes("T")) {
    const date = new Date(value);
    if (!Number.isNaN(date.getTime())) {
      return date.toLocaleString("es-DO");
    }
  }
  return String(value);
}

function fillForm(row) {
  state.editingId = row.id;
  el.entityForm.elements.id.value = row.id;

  const config = schema[state.activeEntity];
  config.fields.forEach((field) => {
    const input = el.entityForm.elements[field.name];
    if (!input) return;

    let value = row[field.name] ?? "";
    if (field.type === "datetime-local" && value) {
      value = toDatetimeLocal(value);
    }
    if (field.type === "date" && value) {
      value = toDateOnly(value);
    }
    input.value = value;
  });

  el.formTitle.textContent = `Editando ${state.activeEntity} #${row.id}`;
}

function clearForm() {
  state.editingId = null;
  el.entityForm.reset();
  el.entityForm.elements.id.value = "";
  el.formTitle.textContent = `Formulario ${state.activeEntity}`;
}

async function onSubmitEntity(event) {
  event.preventDefault();
  if (!requireAdmin()) return;

  const config = schema[state.activeEntity];
  const body = buildBody(config.fields);

  if (state.editingId) {
    await updateEntity(config, state.editingId, body);
  } else {
    await createEntity(config, body);
  }
}

function buildBody(fields) {
  const body = {};

  fields.forEach((field) => {
    const input = el.entityForm.elements[field.name];
    if (!input) return;

    let value = input.value;
    if (field.type === "number" || field.type === "entity-select") {
      value = value === "" ? null : Number(value);
    }
    if (field.type === "date" && value) {
      value = `${value}T00:00:00`;
    }

    body[field.name] = value;
  });

  return body;
}

async function createEntity(config, body) {
  try {
    await apiFetch(config.endpoint, {
      method: "POST",
      body
    });
    toast(`Creado en ${state.activeEntity}.`, "ok");
    clearForm();
    await loadEntityData();
  } catch (error) {
    toast(error.message, "error");
  }
}

async function updateEntity(config, id, body) {
  try {
    await apiFetch(`${config.endpoint}/${id}`, {
      method: "PUT",
      body
    });
    toast(`Actualizado #${id}.`, "ok");
    clearForm();
    await loadEntityData();
  } catch (error) {
    toast(error.message, "error");
  }
}

async function deleteEntity(id) {
  if (!requireAdmin()) return;
  const yes = confirm(`Eliminar ${state.activeEntity} #${id}?`);
  if (!yes) return;

  try {
    const config = schema[state.activeEntity];
    await apiFetch(`${config.endpoint}/${id}`, { method: "DELETE" });
    toast(`Eliminado #${id}.`, "ok");
    if (state.editingId === id) clearForm();
    await loadEntityData();
  } catch (error) {
    toast(error.message, "error");
  }
}

function requireAdmin() {
  if (!state.auth?.token) {
    toast("Inicia sesion para continuar.", "error");
    return false;
  }

  if (state.auth.rol !== "Admin") {
    toast("Acceso restringido: necesitas rol Admin.", "error");
    return false;
  }

  return true;
}

function mapAuth(raw) {
  return {
    token: raw.token,
    username: raw.username,
    rol: raw.rol,
    nombre: raw.nombre
  };
}

async function apiFetch(path, options = {}) {
  const headers = {
    "Content-Type": "application/json",
    ...(options.headers || {})
  };

  if (state.auth?.token) {
    headers.Authorization = `Bearer ${state.auth.token}`;
  }

  const response = await fetch(`${getApiBase()}/${path}`, {
    method: options.method || "GET",
    headers,
    body: options.body ? JSON.stringify(options.body) : undefined
  });

  if (response.status === 204) {
    return null;
  }

  const data = await safeJson(response);

  if (!response.ok) {
    throw new Error(extractError(data) || `Error HTTP ${response.status}`);
  }

  return data;
}

async function safeJson(response) {
  try {
    return await response.json();
  } catch {
    return null;
  }
}

function extractError(payload) {
  if (!payload) return "Error inesperado";

  if (typeof payload === "string") return payload;
  if (payload.detail) return payload.detail;
  if (payload.title) return payload.title;

  if (payload.errors) {
    const first = Object.values(payload.errors)[0];
    if (Array.isArray(first) && first.length) {
      return first[0];
    }
  }

  return "Error inesperado";
}

function humanize(value) {
  return value
    .replace(/([A-Z])/g, " $1")
    .replace(/^./, (x) => x.toUpperCase())
    .trim();
}

function toDatetimeLocal(isoValue) {
  const date = new Date(isoValue);
  if (Number.isNaN(date.getTime())) return "";

  const offset = date.getTimezoneOffset();
  const local = new Date(date.getTime() - offset * 60000);
  return local.toISOString().slice(0, 16);
}

function toDateOnly(isoValue) {
  const date = new Date(isoValue);
  if (Number.isNaN(date.getTime())) return "";

  const offset = date.getTimezoneOffset();
  const local = new Date(date.getTime() - offset * 60000);
  return local.toISOString().slice(0, 10);
}

let toastTimer = null;
function toast(message, kind = "") {
  el.toast.textContent = message;
  el.toast.className = `toast show ${kind}`;

  if (toastTimer) clearTimeout(toastTimer);
  toastTimer = setTimeout(() => {
    el.toast.className = "toast";
  }, 3000);
}