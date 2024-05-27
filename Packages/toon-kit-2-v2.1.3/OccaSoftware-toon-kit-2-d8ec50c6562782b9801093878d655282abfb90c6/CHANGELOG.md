# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [2.1.3] - 2023-11-17

### Fixed

- Fixed a Shader Graph Preview issue.

## [2.1.2] - 2023-11-07

### Fixed

- Fixed the Threshold additional light parameter.

## [2.1.1] - 2023-07-03

### Fixed

- Fixed an issue in built players causing Additional Lights to break.

## [2.1.0] - 2023-06-21

### Changed

- Toon Lights now use constant falloff within the light's range, which gives more control over which objects they affect.

## [2.0.0] - 2023-05-23

### Changed

- Migrated to Package style format to simplify maintenance across Unity versions

### Added

- Added support for Unity 2022.2+

## [1.4.0] - 2023-05-03

- Added support for Tiling & Offset for Base Map, Normal Map, Roughness Map, and Emissive Map.
- Added support for Additional Light Shadows.

## [1.3.0] - 2023-04-10

- Added support for emissive materials.

## [1.2.0] - 2023-03-09

- Added a property to control shadow tint for each material
- Added a property to receive built-in fog for each material
- Added links to documentation and asset registration in the editor
- Improved editor GUI

## [1.1.0] - 2023-01-17

- Added an option to control the Opacity of Transparent materials. This value is multiplied by the Base Map's alpha channel to set the final Opacity for each fragment.

## [1.0.1] - 2023-01-17

- Fixed an issue where the Render Queue setting would be unresponsive in the Editor GUI.

## [1.0.0] - 2023-01-04

- Initial release
