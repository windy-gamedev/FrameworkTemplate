# 🧩 **__RSUnityFramework__**
> _Reusable modular foundation for Unity projects — by RS Studio_  
> _Last updated: October 22, 2025_

---

## 🌐 Overview
`__RSUnityFramework__` is the **core shared framework** used across all Unity projects at RS Studio.  
It provides a **modular architecture**, standardized **service layer**, and **utility toolkit** to accelerate development and ensure consistency between projects.

---

## 📁 Folder Structure
__RSUnityFramework__/
├─ Common/ → Base types, extensions, and helper utilities
├─ Core/ → Service Locator, BaseManager, Dependency Injection (optional)
├─ Editor/ → Custom editor tools, inspectors, and menu utilities
├─ Managers/ → Central managers (GameManager, UIManager, SceneManager)
├─ Services/ → Global services (Audio, Save, Analytics, RemoteConfig, Localization)
├─ UI/ → Base UI logic and reusable components
├─ Utilities/ → Generic tools (Math, Event, Coroutine, Tween, FileIO)
├─ Plugins/ → Third-party dependencies (DOTween, Odin, Firebase, etc.)

---

## ⚙️ Module Details

### 🧱 **Common/**
Contains shared definitions and basic helper classes.
- `Extensions/` – Common C# and Unity extension methods.
- `Helpers/` – Generic helper functions (math, IO, string, etc.)
- `Constants/` – Shared constants and enums.

---

### 🧠 **Core/**
Foundation systems for dependency management and initialization.
- `ServiceLocator` – Centralized access to global services.
- `BaseManager` – Common base for managers.
- `DependencyInjection` – Optional DI or lifetime management system.

> 🧩 This layer connects services, managers, and gameplay systems.

---

### 🧰 **Editor/**
Editor-time utilities and tools.
- Custom inspectors and property drawers.  
- Odin integration scripts.  
- Menu and asset creation tools.

> ⚠️ Automatically excluded from build (Editor-only).

---

### 🎮 **Managers/**
Core managers that control runtime systems.
- `GameManager` – Entry point for the main loop.
- `UIManager` – Handles screen transitions and popup flow.
- `SceneManager` – Manages scene transitions and loading.

> Each manager should remain game-agnostic and reusable.

---

### 🌍 **Services/**
Independent systems that manage cross-game data or logic:
- `AudioService` – Music and SFX management.
- `SaveService` – Player save & load logic.
- `AnalyticsService` – Tracking and analytics wrappers.
- `RemoteConfigService` – Runtime configuration management.
- `LocalizationService` – Multi-language text and asset support.

> All services implement a common interface (e.g. `IService`)  
> and are registered via `ServiceLocator` or `ServiceManager`.

---

### 🖼️ **UI/**
Base UI components and view logic.
- `UIBase` – Base for all UI screens.
- `UIComponent` – Reusable parts (buttons, progress bars, etc.)
- `PopupBase` – Modal/popup window base.

> Recommended architecture: **MVVM** or **MVC** pattern.

---

### 🔧 **Utilities/**
General-purpose reusable utilities:
- `Math/` – Math helpers, interpolation, random utils.
- `Time/` – Timer, cooldown helpers.
- `Event/` – Event bus, observer system.
- `Tween/` – DOTween helpers or custom tweens.
- `Coroutine/` – Static coroutine runner.
- `FileIO/` – File read/write, JSON, and serialization tools.

> Should be lightweight and game-independent.

---

### 📦 **Plugins/**
Third-party libraries and SDKs integrated into the project:
- `DOTween/` – Tweening engine.
- `OdinInspector/` – Editor enhancements.
- `Firebase/`, `GameAnalytics/`, etc.

> 🧭 Keep third-party code isolated.  
> Wrap their APIs in Services or Utilities for maintainability.

---

### 🧪 **Tests/**
Unit Tests and Play Mode Tests.  
Used to validate framework logic and ensure backward compatibility.

---

## 🧩 Integration Guidelines

1. **Do not modify** plugin or third-party source code.  
   → Use wrapper classes in `Services/` or `Utilities/` instead.

2. **All services** should implement a shared interface (`IService`)  
   and register themselves in the `ServiceLocator`.

3. **Framework updates** must remain **game-agnostic** and **backward compatible**.

4. Keep each module **self-contained** for easier maintenance and versioning.

---

## 🧭 Coding Standards

| Convention | Example |
|-------------|----------|
| **Namespace format** | `RSUnity.Services.Audio`, `RSUnity.UI.Components` |
| **Script naming** | Match class name exactly (PascalCase) |
| **File organization** | One class per file unless strongly related |
| **Data storage** | Prefer `ScriptableObjects` for configuration |
| **Initialization** | Use `[RuntimeInitializeOnLoadMethod]` for auto-registration |

---

## 🧱 Design Philosophy

> _“Write once, reuse everywhere.”_  
> __RSUnityFramework__ is built for **scalability, modularity, and reusability**.  
> Any feature that could be reused in another project should be moved here.

---

## 🏷️ Notes
- `__RSUnityFramework__` should remain **independent of game-specific content**.  
- Designed to be imported or version-controlled as a shared module.  
- Ensure compatibility with Unity **2022.3+ (LTS)** or higher.

---

### © RS Studio – Internal Framework Documentation
Maintained by the RS Core Engineering Team  
_This document is intended for internal use only._
