# QRClip

Génère instantanément un QR Code à partir du presse-papiers via un raccourci clavier global.  
Idéal pour envoyer rapidement un lien de votre PC vers votre téléphone.

---

## Fonctionnement

1. Copiez un lien avec `Ctrl+C`
2. Appuyez sur `Ctrl+Shift+Espace`
3. Scannez le QR Code avec votre téléphone
4. Fermez avec `Échap` ou en cliquant ailleurs

L'application tourne en arrière-plan dans le **system tray** (zone de notification Windows).

---

## Prérequis

- Windows 10 ou 11 (64 bits)
- [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) — Desktop Runtime x64

---

## Installation

### Option A — Exécutable autonome (recommandé)

1. Téléchargez `QRClip.exe` depuis la page [Releases](../../releases)
2. Placez le fichier où vous le souhaitez (ex. `C:\Program Files\QRClip\`)
3. Double-cliquez pour lancer — l'icône apparaît dans le system tray

> **Démarrage automatique avec Windows** : clic droit sur l'icône → *Lancer au démarrage*

### Option B — Compiler depuis les sources

**Prérequis supplémentaires :** [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
git clone https://github.com/votre-utilisateur/QRClip.git
cd QRClip
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

L'exécutable se trouve dans :
```
bin\Release\net8.0-windows\win-x64\publish\QRClip.exe
```

---

## Utilisation

| Action | Raccourci |
|--------|-----------|
| Afficher le QR Code | `Ctrl+Shift+Espace` (configurable) |
| Fermer la popup | `Échap` ou clic hors fenêtre |
| Ouvrir le menu | Clic droit sur l'icône tray |
| Afficher le QR (alternative) | Double-clic sur l'icône tray |

### Changer le raccourci clavier

Clic droit sur l'icône tray → **Paramètres**

La configuration est sauvegardée dans `%APPDATA%\QRClip\settings.json`.

---

## Désinstallation

1. Quittez QRClip via le menu tray → *Quitter*
2. Supprimez `QRClip.exe`
3. (Optionnel) Supprimez le dossier de config : `%APPDATA%\QRClip\`

---

## Technologies

- **C# / .NET 8 WPF** — natif Windows, démarrage rapide
- **[QRCoder](https://github.com/codebude/QRCoder)** — génération QR en mémoire (MIT)
- **Win32 `RegisterHotKey`** — raccourci clavier global système

---

## Licence

MIT
